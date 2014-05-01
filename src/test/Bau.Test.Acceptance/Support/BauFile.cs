// <copyright file="BauFile.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Acceptance.Support
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Baufile
    {
        private readonly string scenario;
        private readonly string path;

        private Baufile(string scenario)
        {
            this.scenario = scenario;
            if (Directory.Exists(this.scenario))
            {
                Directory.Delete(this.scenario, true);
            }

            FileSystem.CreateDirectory(this.scenario);

#if DEBUG
            var outputPaths = new[]
            {
                @"..\..\..\..\Bau\bin\Debug",
                @"..\..\..\..\Bau.Exec\bin\Debug",
            };
#else
            var outputPaths = new[]
            {
                @"..\..\..\..\Bau\bin\Release",
                @"..\..\..\..\Bau.Exec\bin\Release",
            };
#endif
            var binPath = Path.Combine(this.scenario, "bin");
            FileSystem.CreateDirectory(binPath);
            foreach (var file in outputPaths.SelectMany(bauOutputPath => Directory.GetFiles(bauOutputPath, "*.dll", SearchOption.TopDirectoryOnly)))
            {
                File.Copy(file, Path.Combine(binPath, Path.GetFileName(file)), true);
            }

            File.Delete(this.path = Path.Combine(this.scenario, "baufile.csx"));
        }

        public static Baufile Create(string scenario)
        {
            return new Baufile(scenario);
        }

        public Baufile Write(string code)
        {
            using (var writer = new StreamWriter(this.path, true))
            {
                writer.Write(code);
                writer.Flush();
            }

            return this;
        }

        public Baufile WriteLine(string code)
        {
            return this.Write(code + Environment.NewLine);
        }

        public string Execute()
        {
            using (var process = new Process())
            {
                var output = new StringBuilder();

                ////var logFile = Path.Combine(Directory.GetCurrentDirectory(), "scriptcs." + this.scenario + ".log");

                process.StartInfo.WorkingDirectory = Path.GetDirectoryName(this.path);
                process.StartInfo.FileName = "scriptcs";
                process.StartInfo.Arguments = "baufile.csx";
                ////process.StartInfo.Arguments = this.path + " -debug -logfile " + logFile; // may be supported in scriptcs > 0.9
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.OutputDataReceived += (sender, args) => output.AppendLine(args.Data);
                process.ErrorDataReceived += (sender, args) => output.AppendLine(args.Data);
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException(output.ToString());
                }

                return output.ToString();
            }
        }
    }
}
