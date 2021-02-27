namespace Consumers
{
    using System;

    public record WeatherData
    {
        public WeatherData(string location, DateTime date, int tempF, string weather, int humidity, int windSpeedMPH)
        {
            Location = location;
            this.date = date;
            TempF = tempF;
            Weather = weather;
            Humidity = humidity;
            WindSpeedMPH = windSpeedMPH;
        }

        public WeatherData(string key, string data)
        {
            Location = key.Split('$')[1];

            var values = data.Split(',');
            date = DateTime.Parse(values[0]);
            TempF = int.Parse(values[1]);
            Weather = values[2];
            Humidity = int.Parse(values[3]);
            WindSpeedMPH = int.Parse(values[4]);
        }

        public string Location { get; }
        public DateTime date { get; }
        public int TempF { get; }
        public string Weather { get; }
        public int Humidity { get; }
        public int WindSpeedMPH { get; }
    }
}
