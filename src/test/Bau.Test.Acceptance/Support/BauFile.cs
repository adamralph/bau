// <copyright file="BauFile.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Acceptance.Support
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    public sealed class Baufile : IDisposable
    {
        ////private readonly string scenario;
        private readonly string folderFullName;
        private readonly string path;

        private Baufile(string scenario)
        {
            ////this.scenario = scenario;
            this.folderFullName = Path.Combine(Path.GetTempPath(), scenario);
            Directory.CreateDirectory(this.folderFullName);

#if DEBUG
            var bauOutputPath = @"..\..\..\..\Bau\bin\Debug";
#else
            var bauOutputPath = @"..\..\..\..\Bau\bin\Release";
#endif

            var binPath = Directory.CreateDirectory(Path.Combine(this.folderFullName, "bin")).FullName;
            foreach (var file in Directory.GetFiles(bauOutputPath, "*.dll", SearchOption.TopDirectoryOnly))
            {
                File.Copy(file, Path.Combine(binPath, Path.GetFileName(file)), true);
            }

            File.Delete(this.path = Path.Combine(this.folderFullName, "baufile.csx"));
        }

        public static Baufile Create(string scenario, bool terminateLine = true)
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
                process.StartInfo.Arguments = this.path;
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

        public void Dispose()
        {
            Directory.Delete(this.folderFullName, true);
        }
    }
}
