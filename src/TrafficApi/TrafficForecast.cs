using System;

namespace TrafficApi
{
    public class TrafficForecast
    {
        public DateTime Date { get; set; }

        public int RoadId { get; set; }
        public int BusyScore { get; set; }
        public string Summary { get; set; }
    }
}
