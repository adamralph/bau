var isMono = Type.GetType("Mono.Runtime") != null;

// parameters
var ci = Environment.GetEnvironmentVariable("CI");
var versionSuffix = Environment.GetEnvironmentVariable("VERSION_SUFFIX");
var msBuildFileVerbosity = (Verbosity)Enum.Parse(typeof(Verbosity), Environment.GetEnvironmentVariable("MSBUILD_FILE_VERBOSITY") ?? "minimal", true);
var nugetVerbosity = Environment.GetEnvironmentVariable("NUGET_VERBOSITY") ?? "quiet";

// solution specific variables
var version = File.ReadAllText("src/CommonAssemblyInfo.cs").Split(new[] { "AssemblyInformationalVersion(\"" }, 2, StringSplitOptions.None).ElementAt(1).Split(new[] { '"' }).First();
var nugetCommand = "scriptcs_packages/NuGet.CommandLine.3.3.0/tools/NuGet.exe";
var xunitCommand = "scriptcs_packages/xunit.runners.1.9.2/tools/xunit.console.clr4.exe";
var solution = "src/Bau.sln";
var output = "artifacts/output";
var tests = "artifacts/tests";
var logs = "artifacts/logs";
var units = new[] { "src/test/Bau.Test.Unit/bin/Release/Bau.Test.Unit.dll", "src/test/Bau.Xunit.Test.Unit/bin/Release/Bau.Xunit.Test.Unit.dll", };
var component = "src/test/Bau.Test.Component/bin/Release/Bau.Test.Component.dll";
var acceptance = "src/test/Bau.Test.Acceptance/bin/Release/Bau.Test.Acceptance.dll";
var packs = new[] { "src/Bau/Bau", "src/Bau.Exec/Bau.Exec", "src/Bau.Xunit/Bau.Xunit", };

// solution agnostic tasks
var bau = Require<Bau>();

bau.Task("default").DependsOn("unit", "component");
if (!isMono)
{
    bau.Task("default").DependsOn("pack");
    if (!string.IsNullOrWhiteSpace(ci))
    {
        bau.Task("default").DependsOn("accept");
    }
}

bau.Task("all").DependsOn("unit", "component");
if (!isMono)
{
    bau.Task("all").DependsOn("accept", "pack");
}

bau.Task("logs").Do(() => CreateDirectory(logs));

if (isMono)
{
    bau.Exec("clean").DependsOn("logs").Do(exec => exec
        .Run("xbuild")
        .With(solution, "/target:Clean", "/property:Configuration=Release", "/verbosity:normal", "/nologo"));
}
else
{
    bau.MSBuild("clean").DependsOn("logs").Do(msb =>
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
    });
}

bau.Task("clobber").DependsOn("clean").Do(() => DeleteDirectory(output));

if (isMono)
{
    bau.Exec("restore").Do(exec => exec
        .Run("mono")
        .With(new [] { nugetCommand, "restore", solution }));
}
else
{
    bau.Exec("restore").Do(exec => exec
        .Run(nugetCommand)
        .With(new [] { "restore", solution }));
}

if (isMono)
{
    bau.Exec("build").Do(exec => exec
        .Run("xbuild")
        .With(solution, "/target:Build", "/property:Configuration=Release", "/verbosity:normal", "/nologo"));
}
else
{
    bau.MSBuild("build").Do(msb =>
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
    });
}

bau.Task("build").DependsOn("clean", "restore", "logs")

.Task("tests").Do(() => CreateDirectory(tests))

.Xunit("unit").DependsOn("build", "tests").Do(xunit => xunit
    .Use(xunitCommand).Run(units).Html().Xml())

.Xunit("component").DependsOn("build", "tests").Do(xunit => xunit
    .Use(xunitCommand).Run(component).Html().Xml())

.Xunit("accept").DependsOn("build", "tests").Do(xunit => xunit
    .Use(xunitCommand).Run(acceptance).Html().Xml())

.Task("output").Do(() => CreateDirectory(output))

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
                bau.CurrentTask.LogInfo("Packing '" + project + "'...");
                
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

void CreateDirectory(string name)
{
    if (!Directory.Exists(name))
    {
        Directory.CreateDirectory(name);
        System.Threading.Thread.Sleep(100); // HACK (adamralph): wait for the directory to be created
    }
}

void DeleteDirectory(string name)
{
    if (Directory.Exists(name))
    {
        Directory.Delete(name, true);
    }
}
