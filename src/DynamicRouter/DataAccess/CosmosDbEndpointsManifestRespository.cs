using DynamicRoutes.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Microsoft.Azure.Cosmos;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace DynamicRoutes.DataAccess
{
    public class CosmosDbEndpointsManifestRespository : BaseEndpointsManifestRespository
    {
        private readonly Container _container;
        private readonly IAppCache _cache;
        private readonly ILogger<CosmosDbEndpointsManifestRespository> _logger;

        class CosmosEntity<T>
        {
            public T Entity { get; set; }
            public string id { get; set; }
        }

        public CosmosDbEndpointsManifestRespository(ILogger<CosmosDbEndpointsManifestRespository> logger, string constr, string db, string container)
        {
            var cosmosClient = new CosmosClient(constr);

            _container = cosmosClient.GetContainer(db, container);
            _cache = new CachingService();
            _logger = logger;
        }


        public override async Task<ApiEndpoint> Add(ApiEndpoint endpoint)
        {
            var recordId = $"{endpoint.Service}#{endpoint.Path.ToClean()}#{endpoint.Port}";
            try
            {
                await _container.CreateItemAsync(new CosmosEntity<ApiEndpoint> { id = recordId, Entity = endpoint });
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Add [{endpoint.ToJson()}] failed");
                throw;
            }
            return endpoint;
        }


        public override async Task<IEnumerable<ApiEndpoint>> GetAll()
        {
            return await _cache.GetOrAddAsync("all", async () =>
            {
                return await readFromDb();
            }, getCacheOptions());
        }

        private MemoryCacheEntryOptions getCacheOptions()
        {
            //ensure the cache item expires exactly on 30s (and not lazily on the next access)
            var options = new LazyCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(30), ExpirationMode.ImmediateEviction);

            // as soon as it expires, re-add it to the cache
            options.RegisterPostEvictionCallback(async (keyEvicted, value, reason, state) =>
            {
                // dont re-add if running out of memory or it was forcibly removed
                if (reason == EvictionReason.Expired || reason == EvictionReason.TokenExpired)
                {
                    await Task.Delay(TimeSpan.FromSeconds(10));
                    await _cache.GetOrAddAsync("all", async (_) => await readFromDb(), getCacheOptions()); //calls itself to get another set of options!
                }
            });
            return options;
        }

        private async Task<IEnumerable<ApiEndpoint>> readFromDb()
        {
            var res = _container.GetItemQueryIterator<CosmosEntity<ApiEndpoint>>("SELECT * from c");
            var items = new List<ApiEndpoint>();
            while (res.HasMoreResults)
            {
                var batchItems = await res.ReadNextAsync();
                items.AddRange(batchItems.Select(e => e.Entity));
            }
            var itms = items.GroupBy(ep => ep.Path.ToClean())
                .Select(grp => getMaxPriorityInGroup(grp))
                .AsEnumerable();
            return itms;
        }
    }
}
