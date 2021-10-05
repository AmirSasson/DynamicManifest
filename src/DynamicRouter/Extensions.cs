using System.Threading.Tasks;

namespace DynamicRoutes
{
    public static class Extensions
    {
        public static string ToClean(this object somePath)
        {
            return somePath.ToString().ToLower().Trim().Trim('/');
        }
        public static Task<TObj> AsTask<TObj>(this TObj any)
        {
            return Task.FromResult(any);
        }
    }
}
