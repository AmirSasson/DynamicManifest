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
|--------------|--------------|
|  WeatherApi is boots,  then another instance of WeatherApi boots | same endpoints are registered|
 | WeatherApi shuts down | nothing, the routes are persisted|
 | new version of WeatherApi Boots | new Apis are registered|
 | WeatherApi is refactored and Some Endpoints moved to NewWeatherApiService | the NewWeatherApiService should registered in higher priority to catch the routes (even on "old" WeatherApi restarts ) |
 

