// <copyright file="BauFile.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

using System.ComponentModel;
using System.Globalization;

namespace Bau.Test.Acceptance.Support
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Text;
    using LiteGuard;

    public class Baufile
    {
        private static readonly string directory = "scenarios";

        private readonly string path;
        private readonly string name;
        private readonly string log;

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "They are initialized inline. The constructor does other things.")]
        static Baufile()
        {
            FileSystem.EnsureDirectoryCreated(directory);
            var bin = Path.Combine(directory, "scriptcs_bin");
            FileSystem.EnsureDirectoryCreated(bin);

            var assemblyDirectories = new[]
            {
#if DEBUG
                @"..\..\..\..\Bau\bin\Debug",
                @"..\..\..\..\Bau.Exec\bin\Debug",
#else
                @"..\..\..\..\Bau\bin\Release",
                @"..\..\..\..\Bau.Exec\bin\Release",
#endif
            };

            foreach (var source in assemblyDirectories.SelectMany(bauOutputPath =>
                System.IO.Directory.GetFiles(bauOutputPath, "*.dll", SearchOption.TopDirectoryOnly)))
            {
                var destination = Path.Combine(bin, Path.GetFileName(source));
                if (!File.Exists(destination) || File.GetLastWriteTimeUtc(source) > File.GetLastWriteTimeUtc(destination))
                {
                    File.Copy(source, destination, true);
                }
            }
        }

        private Baufile(string scenario, bool createWorkingDirectory)
        {
            File.Delete(this.path = Path.Combine(directory, this.name = string.Concat("baufile.", scenario, ".csx")));
            File.Delete(this.log = Path.Combine(directory, string.Concat("baufile.", scenario, ".log")));

            if (createWorkingDirectory)
            {
                FileSystem.EnsureDirectoryDeleted(Path.Combine(directory, scenario));
                FileSystem.EnsureDirectoryCreated(Path.Combine(directory, scenario));
            }
        }

        public static string Directory
        {
            get { return directory; }
        }

        public static Baufile Create(string scenario, bool createWorkingDirectory = false)
        {
            return new Baufile(scenario, createWorkingDirectory);
        }

        public Baufile WriteLine(string code)
        {
            using (var writer = new StreamWriter(this.path, true))
            {
                writer.WriteLine(code);
                writer.Flush();
            }

            return this;
        }

        public string Run(params string[] arguments)
        {
            Guard.AgainstNullArgument("tasks", arguments);

            var args = new List<string> { this.name, "-debug" };
            if (arguments.Length > 0)
            {
                args.Add("--");
                args.AddRange(arguments);
            }

            var info = new ProcessStartInfo
            {
                FileName = Environment.GetEnvironmentVariable("SCRIPTCS_PATH") ?? "scriptcs",
                Arguments = string.Join(" ", args),
                WorkingDirectory = directory,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var output = new StringBuilder();
            using (var process = new Process())
            {
                process.StartInfo = info;
                process.OutputDataReceived += (sender, e) => output.AppendLine(e.Data);
                process.ErrorDataReceived += (sender, e) => output.AppendLine(e.Data);
                try
                {
                    process.Start();
                }
                catch (Win32Exception ex)
                {
                    if (ex.NativeErrorCode == 2)
                    {
                        var message =
                            string.Format(CultureInfo.InvariantCulture, "Could not find file '{0}'.", info.FileName);

                        throw new FileNotFoundException(message, info.FileName, ex);
                    }

                    throw;
                }

                process.BeginOutputReadLine();
                process.WaitForExit();

                using (var writer = new StreamWriter(this.log, true))
                {
                    writer.WriteLine(output.ToString());
                    writer.Flush();
                }

                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException(output.ToString());
                }
            }

            return output.ToString();
        }
    }
}
