



// To build with Bau:
// 1. install scriptcs: http://chocolatey.org/packages/ScriptCs
// 2. install packages: scriptcs -install
// 3. execute baufile:  scriptcs baufile.csx


var nugetCommand = "packages/NuGet.CommandLine.2.8.3/tools/NuGet.exe";
var xunitCommand = "packages/xunit.runners.1.9.2/tools/xunit.console.clr4.exe";
var solution = "../src/Bau.sln";
var test = "../src/test/Bau.Test.Component/bin/Release/Bau.Test.Component.dll";

Require<Bau>()

.Task("default").DependsOn("test")

.MSBuild("clean").Do(msbuild =>
{
    msbuild.MSBuildVersion = "net45";
    msbuild.Solution = solution;
    msbuild.Targets = new[] { "Clean"};
    msbuild.Properties = new { Configuration = "Release", };
    msbuild.Verbosity = Verbosity.Minimal;
    msbuild.NoLogo = true;
})

.Exec("restore").Do(exec => exec
    .Run(nugetCommand).With("restore", solution))

.MSBuild("build").DependsOn("clean", "restore").Do(msbuild =>
{
    msbuild.MSBuildVersion = "net45";
    msbuild.Solution = solution;
    msbuild.Targets = new[] { "Build"};
    msbuild.Properties = new { Configuration = "Release", };
    msbuild.Verbosity = Verbosity.Minimal;
    msbuild.NoLogo = true;
})

.Xunit("test").DependsOn("build").Do(xunit => xunit
    .Use(xunitCommand).Run(test).Html().Xml())

.Run();
