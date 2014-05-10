var version = File.ReadAllText("src/CommonAssemblyInfo.cs").Split(new[] { "AssemblyInformationalVersion(\"" }, 2, StringSplitOptions.None).ElementAt(1).Split(new[] { '"' }).First();
var versionSuffix = Environment.GetEnvironmentVariable("VERSION_SUFFIX");
var msBuildCommand = Path.Combine(Environment.GetEnvironmentVariable("WINDIR"), "Microsoft.NET/Framework/v4.0.30319/MSBuild.exe");
var nugetCommand = "packages/NuGet.CommandLine.2.8.1/tools/NuGet.exe";
var nugetVerbosity = Environment.GetEnvironmentVariable("NUGET_VERBOSITY"); if (string.IsNullOrWhiteSpace(nugetVerbosity)) { nugetVerbosity = "quiet"; };
var xunitCommand = "packages/xunit.runners.1.9.2/tools/xunit.console.clr4.exe";
var solution = "src/Bau.sln";
var output = "artifacts/output";
var logs = "artifacts/logs";
var component = "src/test/Bau.Test.Component/bin/Release/Bau.Test.Component.dll";
var acceptance = "src/test/Bau.Test.Acceptance/bin/Release/Bau.Test.Acceptance.dll";
var packs = new[] { "src/Bau/Bau", "src/Bau.Exec/Bau.Exec", };

Require<Bau>()

.Task("default").DependsOn("component", "accept", "pack")

.Task("logs").Do(() =>
    {
        if (!Directory.Exists(logs))
        {
            Directory.CreateDirectory(logs);
            System.Threading.Thread.Sleep(100); // HACK (adamralph): wait for the directory to be created
        }
    })

.Exec("clean").DependsOn("logs").Do(exec => exec
    .Run(msBuildCommand)
    .With(
        solution,
        "/target:Clean",
        "/property:Configuration=Release",
        "/maxcpucount",
        "/nodeReuse:false",
        "/fileLogger",
        "/fileloggerparameters:PerformanceSummary;Summary;Verbosity=detailed;LogFile=" + logs + "/clean.log",
        "/verbosity:minimal",
        "/nologo"))

.Task("clobber").DependsOn("clean").Do(() =>
    {
        if (Directory.Exists(output))
        {
            Directory.Delete(output, true);
        }
    })

.Exec("restore").Do(exec => exec
    .Run(nugetCommand)
    .With("restore", solution))

.Exec("build").DependsOn("clean", "restore", "logs").Do(exec => exec
    .Run(msBuildCommand)
    .With(
        solution,
        "/target:Build",
        "/property:Configuration=Release",
        "/maxcpucount",
        "/nodeReuse:false",
        "/fileLogger",
        "/fileloggerparameters:PerformanceSummary;Summary;Verbosity=detailed;LogFile=" + logs + "/build.log",
        "/verbosity:minimal",
        "/nologo"))

.Exec("component").DependsOn("build").Do(exec => exec
    .Run(xunitCommand)
    .With(component, "/html", component + "TestResults.html", "/xml", component + "TestResults.xml"))

.Exec("accept").DependsOn("build").Do(exec => exec
    .Run(xunitCommand)
    .With(acceptance, "/html", acceptance + "TestResults.html", "/xml", acceptance + "TestResults.xml"))

.Task("pack").DependsOn("build", "clobber").Do(() =>
    {
        System.Threading.Thread.Sleep(100); // HACK (adamralph): wait for the directory to be deleted in clobber
        Directory.CreateDirectory(output);

        foreach (var pack in packs)
        {
            File.Copy(pack + ".nuspec", pack + ".nuspec.original", true);
        }

        try
        {
            foreach (var pack in packs)
            {
                File.WriteAllText(pack + ".nuspec", File.ReadAllText(pack + ".nuspec").Replace("0.0.0", version + versionSuffix));
                new Exec()
                    .Run(nugetCommand)
                    .With(
                        "pack", pack + ".csproj",
                        "-OutputDirectory", output,
                        "-Properties", "Configuration=Release",
                        "-IncludeReferencedProjects",
                        "-Verbosity " + nugetVerbosity)
                    .Execute();
            }
        }
        finally
        {
            foreach (var pack in packs)
            {
                File.Copy(pack + ".nuspec.original", pack + ".nuspec", true);
                File.Delete(pack + ".nuspec.original");
            }
        }
    })

.Execute();
