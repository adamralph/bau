// 1. install scriptcs: http://chocolatey.org/packages/ScriptCs
// 2. install packages: scriptcs -install
// 3. execute baufile: scriptcs baufile.csx



var msBuildCommand = Path.Combine(Environment.GetEnvironmentVariable("WINDIR"), "Microsoft.NET/Framework/v4.0.30319/MSBuild.exe");
var nugetCommand = "packages/NuGet.CommandLine.2.8.1/tools/NuGet.exe";
var xunitCommand = "packages/xunit.runners.1.9.2/tools/xunit.console.clr4.exe";
var solution = "../src/Bau.sln";
var component = "../src/test/Bau.Test.Component/bin/Release/Bau.Test.Component.dll";
var acceptance = "../src/test/Bau.Test.Acceptance/bin/Release/Bau.Test.Acceptance.dll";

Require<Bau>()

.Task("default").DependsOn("component", "accept")

.Exec("clean").Do(exec => exec
    .Run(msBuildCommand)
    .With(solution, "/target:Clean", "/property:Configuration=Release"))

.Exec("restore").Do(exec => exec
    .Run(nugetCommand)
    .With("restore", solution))

.Exec("build").DependsOn("clean", "restore").Do(exec => exec
    .Run(msBuildCommand)
    .With(solution, "/target:Build", "/property:Configuration=Release"))

.Exec("component").DependsOn("build").Do(exec => exec
    .Run(xunitCommand)
    .With(component, "/html", component + "TestResults.html", "/xml", component + "TestResults.xml"))

.Exec("accept").DependsOn("build").Do(exec => exec
    .Run(xunitCommand)
    .With(acceptance, "/html", acceptance + "TestResults.html", "/xml", acceptance + "TestResults.xml"))

.Execute();
