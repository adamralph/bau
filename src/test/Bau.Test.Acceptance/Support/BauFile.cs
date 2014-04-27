// <copyright file="TempFolder.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Acceptance.Support
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    public sealed class BauFile : IDisposable
    {
        private readonly string folderFullName;
        private readonly string path;

        private BauFile(string code, string scenario)
        {
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

            using (var writer = new StreamWriter(this.path = Path.Combine(this.folderFullName, "baufile.csx")))
            {
                writer.Write(code);
                writer.Flush();
            }
        }

        public static BauFile Create(string code, string scenario)
        {
            return new BauFile(code, scenario);
        }

        public string Execute()
        {
            var process = new Process();
            var output = new StringBuilder();

            process.StartInfo.WorkingDirectory = Path.GetDirectoryName(this.path);
            process.StartInfo.FileName = "scriptcs";
            process.StartInfo.Arguments = this.path;
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
                throw new Exception(output.ToString());
            }

            return output.ToString();
        }

        public void Dispose()
        {
            Directory.Delete(this.folderFullName, true);
        }
    }
}
