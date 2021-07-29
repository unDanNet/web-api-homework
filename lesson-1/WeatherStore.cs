using System;
using System.Collections.Generic;

namespace lesson_1
{
    public class WeatherStore
    {
        public Dictionary<DateTime, WeatherForecast> data { get; set; } = new Dictionary<DateTime, WeatherForecast>();
    }
}