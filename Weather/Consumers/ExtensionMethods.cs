namespace Consumers
{
    using System.Collections.Generic;
    using Weather.Domain;

    public static class ExtensionMethods
    {
        public static string PrettyFormat(this WeatherData _this)
        {
            return $"Location={_this.Location}, Temp={_this.TempF}";
        }


        public static string PrettyFormat<T>(this IEnumerable<T> source) => string.Join(", ", source);
    }
}
