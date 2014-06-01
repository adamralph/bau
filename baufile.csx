// parameters
var ci = Environment.GetEnvironmentVariable("CI");
var versionSuffix = Environment.GetEnvironmentVariable("VERSION_SUFFIX");
var msBuildFileVerbosity = (Verbosity)Enum.Parse(typeof(Verbosity), Environment.GetEnvironmentVariable("MSBUILD_FILE_VERBOSITY") ?? "minimal", true);
var nugetVerbosity = Environment.GetEnvironmentVariable("NUGET_VERBOSITY") ?? "quiet";

// solution specific variables
var version = File.ReadAllText("src/CommonAssemblyInfo.cs").Split(new[] { "AssemblyInformationalVersion(\"" }, 2, StringSplitOptions.None).ElementAt(1).Split(new[] { '"' }).First();
var nugetCommand = "packages/NuGet.CommandLine.2.8.2/tools/NuGet.exe";
var xunitCommand = "packages/xunit.runners.1.9.2/tools/xunit.console.clr4.exe";
var solution = "src/Bau.sln";
var output = "artifacts/output";
var tests = "artifacts/tests";
var logs = "artifacts/logs";
var units = new[] { "src/test/Bau.Test.Unit/bin/Release/Bau.Test.Unit.dll", "src/test/Bau.Xunit.Test.Unit/bin/Release/Bau.Xunit.Test.Unit.dll", };
var component = "src/test/Bau.Test.Component/bin/Release/Bau.Test.Component.dll";
var acceptance = "src/test/Bau.Test.Acceptance/bin/Release/Bau.Test.Acceptance.dll";
var packs = new[] { "src/Bau/Bau", "src/Bau.Exec/Bau.Exec", "src/Bau.Xunit/Bau.Xunit", };

// solution agnostic tasks
Require<Bau>()

.Task("default").DependsOn(string.IsNullOrWhiteSpace(ci) ? new[] { "unit", "component", "pack" } : new[] { "unit", "component", "accept", "pack" })

.Task("all").DependsOn("unit", "component", "accept", "pack")

.Task("logs").Do(() =>
    {
        if (!Directory.Exists(logs))
        {
            Directory.CreateDirectory(logs);
            System.Threading.Thread.Sleep(100); // HACK (adamralph): wait for the directory to be created
        }
    })

.MSBuild("clean").DependsOn("logs").Do(msb =>
    {
        msb.MSBuildVersion = "net45";
        msb.Solution = solution;
        msb.Targets = new[] { "Clean", };
        msb.Properties = new { Configuration = "Release" };
        msb.MaxCpuCount = -1;
        msb.NodeReuse = false;
        msb.Verbosity = msBuildFileVerbosity;
        msb.NoLogo = true;
        msb.FileLoggers.Add(
            new FileLogger
            {
                FileLoggerParameters = new FileLoggerParameters
                {
                    PerformanceSummary = true,
                    Summary = true,
                    Verbosity = Verbosity.Minimal,
                    LogFile = logs + "/clean.log",
                }
            });
    })

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

.MSBuild("build").DependsOn("clean", "restore", "logs").Do(msb =>
    {
        msb.MSBuildVersion = "net45";
        msb.Solution = solution;
        msb.Targets = new[] { "Build", };
        msb.Properties = new { Configuration = "Release" };
        msb.MaxCpuCount = -1;
        msb.NodeReuse = false;
        msb.Verbosity = msBuildFileVerbosity;
        msb.NoLogo = true;
        msb.FileLoggers.Add(
            new FileLogger
            {
                FileLoggerParameters = new FileLoggerParameters
                {
                    PerformanceSummary = true,
                    Summary = true,
                    Verbosity = Verbosity.Minimal,
                    LogFile = logs + "/build.log",
                }
            });
    })

.Task("tests").Do(() =>
    {
        if (!Directory.Exists(tests))
        {
            Directory.CreateDirectory(tests);
            System.Threading.Thread.Sleep(100); // HACK (adamralph): wait for the directory to be created
        }
    })

.Task("unit").DependsOn("build", "tests").Do(() =>
    {
        foreach (var unit in units)
        {
            new Exec { Name = "unit " + unit }
                .Run(xunitCommand)
                .With(unit, "/html", GetTestResultsPath(tests, unit, "html"), "/xml", GetTestResultsPath(tests, unit, "xml"))
                .Execute();
        }
    })

.Exec("component").DependsOn("build", "tests").Do(exec => exec
    .Run(xunitCommand)
    .With(component, "/html", GetTestResultsPath(tests, component, "html"), "/xml", GetTestResultsPath(tests, component, "xml")))

.Exec("accept").DependsOn("build", "tests").Do(exec => exec
    .Run(xunitCommand)
    .With(acceptance, "/html", GetTestResultsPath(tests, acceptance, "html"), "/xml", GetTestResultsPath(tests, acceptance, "xml")))

.Task("output").Do(() =>
    {
        if (!Directory.Exists(output))
        {
            Directory.CreateDirectory(output);
            System.Threading.Thread.Sleep(100); // HACK (adamralph): wait for the directory to be created
        }
    })

.Task("pack").DependsOn("build", "clobber", "output").Do(() =>
    {
        foreach (var pack in packs)
        {
            File.Copy(pack + ".nuspec", pack + ".nuspec.original", true);
        }

        try
        {
            foreach (var pack in packs)
            {
                File.WriteAllText(pack + ".nuspec", File.ReadAllText(pack + ".nuspec").Replace("0.0.0", version + versionSuffix));

                var project = pack + ".csproj";
                new Exec { Name = "pack " + project }
                    .Run(nugetCommand)
                    .With(
                        "pack", project,
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

.Run();

string GetTestResultsPath(string directory, string assembly, string extension)
{
    return Path.GetFullPath(
        Path.Combine(
            directory,
            string.Concat(
                Path.GetFileNameWithoutExtension(assembly),
                ".TestResults.",
                extension)));
}
