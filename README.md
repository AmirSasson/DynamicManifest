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
