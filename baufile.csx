using System.Diagnostics;

var version = File.ReadAllText("src/CommonAssemblyInfo.cs").Split(new[] { "AssemblyInformationalVersion(\"" }, 2, StringSplitOptions.None).ElementAt(1).Split(new[] { '"' }).First();
var nugetCommand = @"packages\NuGet.CommandLine.2.8.1\tools\NuGet.exe";
var xunitCommand = @"packages\xunit.runners.1.9.2\tools\xunit.console.clr4.exe";
var solution = @"src\Bau.sln";
var output = "artifacts";
var acceptance = @"src\test\Bau.Test.Acceptance\bin\Release\Bau.Test.Acceptance.dll";
var nuspec = @"src\Bau\Bau.csproj";

var bau = Require<BauPack>();

// until we have dependencies in place, do everything in one task. dogfooding from the start!
bau.Task(
    "default",
    () =>
    {
        // clean
        if (Directory.Exists(output))
        {
            Directory.Delete(output, true);
        }
        
        using (var process = new Process())
        {
            process.StartInfo.FileName = Path.Combine(Environment.GetEnvironmentVariable("WINDIR"), @"Microsoft.NET\Framework\v4.0.30319\MSBuild.exe");
            process.StartInfo.Arguments = solution + " /target:Clean /property:Configuration=Release";
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new Exception();
            }
        }

        // restore
        using (var process = new Process())
        {
            process.StartInfo.FileName = nugetCommand;
            process.StartInfo.Arguments = "restore " + solution;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new Exception();
            }
        }

        // build
        using (var process = new Process())
        {
            process.StartInfo.FileName = Path.Combine(Environment.GetEnvironmentVariable("WINDIR"), @"Microsoft.NET\Framework\v4.0.30319\MSBuild.exe");
            process.StartInfo.Arguments = solution + " /target:Build /property:Configuration=Release";
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new Exception();
            }
        }

        // accept
        using (var process = new Process())
        {
            process.StartInfo.FileName = xunitCommand;
            process.StartInfo.Arguments = acceptance + " /html " + acceptance + "TestResults.html" + " /xml " + acceptance + "TestResults.xml";
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new Exception();
            }
        }

        // pack
        Directory.CreateDirectory(output);
        using (var process = new Process())
        {
            process.StartInfo.FileName = nugetCommand;
            process.StartInfo.Arguments = "pack " + nuspec + " -Version " + version + " -OutputDirectory " + output + " -Properties Configuration=Release";
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new Exception();
            }
        }
    });
    
bau.Execute();
