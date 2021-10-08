# DynamicManifest
short ASPNET core apps that behaves as facade to outer world.
the DynamicRoutes app has a DB of all registered endpoints, and upon request it routes the request to right API.  
see diagram

```
OUTER World      DynamicRoutes               WeatherApi    TrafficApi
  +                   +                          +             +
  |                   |  Register                |             |
  |                   <--------------------------+             |
  |                   |                     Register           |
  |                   +<-------------------------+-------------+
  |                   |                          |             |
  |    GetWeather     |                          |             |
  +------------------>+         GetWeather       |             |
  |                   +------------------------->+             |
  |                   |                          |             |
  |    GetTraffic     |                          |             |
  +-----------------> |                          |             |
  |                   |                    Get Traffic         |
  |                   +--------------------------+------------>+
  +                   +                          +             +

```

# Scenarios:
|Scenario|Behavior|
| :---: | :---: |
|  WeatherApi boots,  then another instance of WeatherApi boots | same endpoints are registered|
 | WeatherApi shuts down | nothing, the routes are persisted|
 | one of the instances in DynamicManifest restarts  | nothing, the routes are persisted, the node aquires the routes on load|
 | new version of WeatherApi Boots | new Apis are registered|
 | IP of WeatherApi is changed | dont care, it is identified by its Service/cluster name/ip no by single node|
 | 2 services "old" and "new" are booting in same fraction of second | Handled by priority, "Concurrency conflict"/"race conditions" wont happen. both records (all regsitered record) are written to ManifestDb, but when fetching, we choose the right ones by priority|
 | WeatherApi is refactored and Some Endpoints moved to NewWeatherApiService | the NewWeatherApiService should registered in higher priority to catch the routes (even on "old" WeatherApi restarts ) |
 | Unregister a mistake endpoint| the "correct" endpoint should be register with higher priority(code change) OR this can be fixed manually in DB |
 |  illegal scenario - The system identify more than 1 route that match a request on different services | illegal scenario - the system should fire an alert that should be monitored  |
  |  Migrating to Different app/docker orchestrator | concept stays the same, the register should include the details to approach itself, on MSFT service fabric- it would be the Service Name + port, on K8s - the **cluster** IP/dns + port|
 
 

 
 # Risks
* persistency
* Performance 
* Rollout 
* Effort
 
 ## Risks Mitagating
  ### Performance aspect
 Each node on DynamicManifest saves a copy of the Enpoints manifest in memory, from the shared persisted storage (Manifest DB), and the middleware saves the fallback into controllers. so upon HttpCall ,no IO is needed to calculate the route. (memory copy can refresh every X minutes async)
 ### persistency 
  * choose carefully a reliable fault tollerant  
    * good example: any X-Sql (MS,MY,posgress), Cosmos/dynamoDb/mongo, (Redis only with persistancy)
    * Bad example: Memcache, Redis (default with no persistency)
 ### Gradual rollout
 * the Implementation is that Endpoint OPT-IN to use the feature, so this minimize the risk before adoption.
 * DynamicRoutes can implement a fallback of controller, and by Rollout feature toggle, you can decide if to delagate to internal apis form a dedicated controller or from the Middle.  (Also can be achieved by identifing a flaf in the request querystring.) 
 ### Effort
 after deciding to adopt, you can switch the OPT-IN feature to ON by default. this would register all Endpoints on you app, with no efforts. 
 
 
 
 

