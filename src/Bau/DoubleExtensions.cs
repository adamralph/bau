// <copyright file="DoubleExtensions.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System.Globalization;

    public static class DoubleExtensions
    {
        public static string ToStringFromMilliseconds(this double milliseconds)
        {
            // nanoseconds
            if (milliseconds < 0.001d)
            {
                return (milliseconds * 1000000d).ToString("G3", CultureInfo.InvariantCulture) + " ns";
            }

            // microseconds
            if (milliseconds < 1d)
            {
                return (milliseconds * 1000d).ToString("G3", CultureInfo.InvariantCulture) + " µs";
            }

            // milliseconds
            if (milliseconds < 1000d)
            {
                return milliseconds.ToString("G3", CultureInfo.InvariantCulture) + " ms";
            }

            // seconds
            if (milliseconds < 60000d)
            {
                return (milliseconds / 1000d).ToString("G3", CultureInfo.InvariantCulture) + " s";
            }

            // minutes and seconds
            if (milliseconds < 3600000d)
            {
                var minutes = (milliseconds / 60000d).ToString("F0", CultureInfo.InvariantCulture);
                var seconds = ((milliseconds % 60000d) / 1000d).ToString("F0", CultureInfo.InvariantCulture);
                return seconds == "0"
                    ? minutes + " min"
                    : string.Concat(minutes, " min ", seconds, " s");
            }

            // minutes
            return (milliseconds / 60000d).ToString("N0", CultureInfo.InvariantCulture) + " min";
        }
    }
}
