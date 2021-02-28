namespace Consumers
{
    using System.Collections.Generic;

    public static class ExtensionMethods
    {
        public static string PrettyFormat<T>(this IEnumerable<T> source) => string.Join(", ", source);
    }
}
