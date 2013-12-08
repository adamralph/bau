// <copyright file="BaufileFinder.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Common.Logging;
    using ServiceStack.Text;

    public static class BaufileFinder
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        public static string Find()
        {
            var filenames = new[] { "baufile", "Baufile", "baufile.csx", "Baufile.csx" };
            log.DebugFormat(CultureInfo.InvariantCulture, "Searching for {0}.", filenames.ToJsv());

            var directory = Directory.GetCurrentDirectory();
            while (true)
            {
                foreach (var filename in filenames.Select(name => Path.Combine(directory, name)))
                {
                    if (File.Exists(filename))
                    {
                        log.DebugFormat(CultureInfo.InvariantCulture, "Found Baufile '{0}'...", filename);
                        return filename;
                    }
                }

                log.TraceFormat(CultureInfo.InvariantCulture, "No Baufile found in directory '{0}'.", directory);
                var parent = Directory.GetParent(directory);
                if (parent == null)
                {
                    break;
                }

                directory = parent.FullName;
            }

            var message =
                string.Format(CultureInfo.InvariantCulture, "No Baufile found (looking for {0}).", filenames.ToJsv());

            throw new FileNotFoundException(message);
        }
    }
}
