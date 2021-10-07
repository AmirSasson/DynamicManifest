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
 | WeatherApi is refactored and Some Endpoints moved to NewWeatherApiService | the NewWeatherApiService should registered in higher priority to catch the routes (even on "old" WeatherApi restarts ) |
 | Unregister a mistake endpoint| the "correct" endpoint should be register with higher priority(code change) OR this can be fixed manually in DB |
 |  illegal scenario - THe system identify more than 1 route that match a request on different services | illegal scenario - the system should fire an alert that should be monitored  |
  |  Migrating to Different app/docker orcestrator | concept stays the same, the register should include the details to approach itself, on MSFT service fabric- it would be the Service Name + port, on K8s - the **cluster** IP+ port|
 
 
 # Performance aspect
 Each node on DynamicManifest saves a copy of the Enpoints manifest in memory, from the shared persisted storage (Manifest DB), and the middleware saves the fallback into controllers. so upon HttpCall ,no IO is needed to calculate the route. (memory copy can refresh every X minutes async)
 
 
 
 
 
 

