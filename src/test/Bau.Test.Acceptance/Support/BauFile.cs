// <copyright file="BauFile.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Acceptance.Support
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Baufile
    {
        private static readonly string directory = "scenarios";

        private readonly string path;
        private readonly string name;
        private readonly string log;

        static Baufile()
        {
            FileSystem.EnsureDirectoryCreated(directory);
            var bin = Path.Combine(directory, "bin");
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

        public string Execute(params string[] tasks)
        {
            var info = new ProcessStartInfo();
            info.FileName = "scriptcs";

            var args = new List<string>();
            args.Add(this.name);
            args.Add("-debug");

            if (tasks.Length > 0)
            {
                args.Add("--");
                args.AddRange(tasks);
            }

            info.Arguments = string.Join(" ", args);
            info.WorkingDirectory = directory;
            info.UseShellExecute = false;
            info.CreateNoWindow = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;

            var output = new StringBuilder();
            using (var process = new Process())
            {
                process.StartInfo = info;
                process.OutputDataReceived += (sender, e) => output.AppendLine(e.Data);
                process.ErrorDataReceived += (sender, e) => output.AppendLine(e.Data);
                process.Start();
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
