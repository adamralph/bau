// <copyright file="FileSystem.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Acceptance.Support
{
    using System;
    using System.Globalization;
    using System.IO;

    public static class FileSystem
    {
        // NOTE (adamralph): difficult to believe that I have to write this, but I do. System.IO and the filesystem race.
        public static void CreateDirectory(string path)
        {
            var timeout = 0.05d;

            var createTimeout = DateTime.Now.AddSeconds(timeout);
            while (true)
            {
                try
                {
                    Directory.CreateDirectory(path);

                    var existsTimeout = DateTime.Now.AddSeconds(timeout);
                    while (true)
                    {
                        if (Directory.Exists(path))
                        {
                            break;
                        }

                        if (DateTime.Now < existsTimeout)
                        {
                            continue;
                        }

                        throw new IOException(
                            string.Format(CultureInfo.InvariantCulture, "Failed to create folder '{0}'", path));
                    }
                }
                catch (Exception)
                {
                    if (DateTime.Now < createTimeout)
                    {
                        continue;
                    }

                    throw;
                }

                break;
            }
        }
    }
}
