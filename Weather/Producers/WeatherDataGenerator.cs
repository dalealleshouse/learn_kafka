namespace Producers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Weather.Domain;

    public enum Location
    {
        [Description("Pittsburgh-PA-US")]
        PA_US,
        [Description("San Francisco-CA-US")]
        SF_US,
        [Description("Sydney-NSW-AU")]
        NSW_AU,
        [Description("Mumbai-NH-IN")]
        NH_IN,
        [Description("London-UK-GB")]
        UK_GB
    };

    public static class WeatherDataGenerator
    {
        private static Dictionary<Location, IList<int>> TempRangesF = new()
        {
            { Location.PA_US, Enumerable.Range(65, 95).ToList() },
            { Location.SF_US, Enumerable.Range(50, 80).ToList() },
            { Location.NSW_AU, Enumerable.Range(45, 75).ToList() },
            { Location.NH_IN, Enumerable.Range(70, 100).ToList() },
            { Location.UK_GB, Enumerable.Range(45, 75).ToList() }
        };

        private static Dictionary<Location, int> LocationOffsetHours = new()
        {
            { Location.PA_US, 0 },
            { Location.SF_US, -3 },
            { Location.NSW_AU, 14 },
            { Location.NH_IN, 9 },
            { Location.UK_GB, 6 }
        };

        private static IList<string> WeatherType = new string[] {
            "Sunny", "Cloudy", "Fog", "Rain", "Lightning", "Windy" };
        private static IList<int> Humidities = Enumerable.Range(30, 100).ToList();
        private static IList<int> WindSpeedMPH = Enumerable.Range(0, 20).ToList();

        private static int TempIndex(Location location, int baseHour, int currentDay)
        {
            int currentTempIndex = (baseHour > 12) ? 12 - (baseHour % 12) : baseHour;
            return (currentTempIndex + currentDay) % TempRangesF[location].Count;
        }

        public static IEnumerable<(Location, WeatherData)> GetWeather(DateTime date, int currentGeneration)
        {
            var locations = (IEnumerable<Location>)Enum.GetValues(typeof(Location));
            foreach (var (location, index) in locations.WithIndex())
            {
                int currentDay = currentGeneration / 24;
                int baseCurrentHour = (currentGeneration + LocationOffsetHours[location]) % 24;
                baseCurrentHour = (baseCurrentHour < 0) ? 0 : baseCurrentHour;
                int currentTempIndex = TempIndex(location, baseCurrentHour, currentDay);

                var temp = TempRangesF[location][currentTempIndex];
                var weather = WeatherType[(currentDay + index) % WeatherType.Count];
                var humidity = Humidities[(currentDay + baseCurrentHour + index) % Humidities.Count];
                var windSpeed = WindSpeedMPH[(currentDay + (baseCurrentHour / 12) + index) % WindSpeedMPH.Count];

                var data = new WeatherData()
                {
                    Date = date.ToString(),
                    Location = location.GetDescription(),
                    TempF = temp,
                    Weather = weather,
                    Humidity = humidity,
                    WindSpeedMPH = windSpeed
                };

                yield return (location, data);
            }
        }
    }
}
