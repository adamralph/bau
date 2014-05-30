Require<Bau>()

.Task("default").DependsOn("unit", "component")

.Task("logs").Do(() =>
    {
        if (!Directory.Exists("artifacts/logs"))
        {
            Directory.CreateDirectory("artifacts/logs");
            System.Threading.Thread.Sleep(100); // HACK (adamralph): wait for the directory to be created
        }
    })

.Exec("clean").DependsOn("logs").Do(exec => exec
    .Run("xbuild")
    .With("src/Bau.sln",
        "/target:Clean",
        "/property:Configuration=Release",
        "/property:StyleCopEnabled=false",
        "/verbosity:minimal",
        "/nologo")
    )

.Exec("restore").Do(exec => exec
    .Run("mono")
    .With("packages/NuGet.CommandLine.2.8.1/tools/NuGet.exe", "restore", "src/Bau.sln"))

.Exec("build").DependsOn("clean", "restore", "logs").Do(exec => exec
    .Run("xbuild")
    .With("src/Bau.sln",
        "/target:Build",
        "/property:Configuration=Release",
        "/verbosity:minimal",
        "/nologo")
    )

.Task("tests").Do(() =>
    {
        if (!Directory.Exists("artifacts/tests"))
        {
            Directory.CreateDirectory("artifacts/tests");
            System.Threading.Thread.Sleep(100); // HACK (adamralph): wait for the directory to be created
        }
    })

.Exec("unit").DependsOn("build", "tests").Do(exec => exec
    .Run("mono")
    .With(
        "packages/xunit.runners.1.9.2/tools/xunit.console.clr4.exe",
        "src/test/Bau.Test.Unit/bin/Release/Bau.Test.Unit.dll",
        "/html", GetTestResultsPath("artifacts/tests", "src/test/Bau.Test.Unit/bin/Release/Bau.Test.Unit.dll", "html"),
        "/xml", GetTestResultsPath("artifacts/tests", "src/test/Bau.Test.Unit/bin/Release/Bau.Test.Unit.dll", "xml"))
    .In("."))

.Exec("component").DependsOn("build", "tests").Do(exec => exec
    .Run("mono")
    .With(
        "packages/xunit.runners.1.9.2/tools/xunit.console.clr4.exe",
        "src/test/Bau.Test.Component/bin/Release/Bau.Test.Component.dll",
        "/html", GetTestResultsPath("artifacts/tests", "src/test/Bau.Test.Component/bin/Release/Bau.Test.Component.dll", "html"),
        "/xml", GetTestResultsPath("artifacts/tests", "src/test/Bau.Test.Component/bin/Release/Bau.Test.Component.dll", "xml"))
    .In("."))

.Exec("accept").DependsOn("build", "tests").Do(exec => exec
    .Run("mono")
    .With(
        "packages/xunit.runners.1.9.2/tools/xunit.console.clr4.exe",
        "src/test/Bau.Test.Acceptance/bin/Release/Bau.Test.Acceptance.dll",
        "/html", GetTestResultsPath("artifacts/tests", "src/test/Bau.Test.Acceptance/bin/Release/Bau.Test.Acceptance.dll", "html"),
        "/xml", GetTestResultsPath("artifacts/tests", "src/test/Bau.Test.Acceptance/bin/Release/Bau.Test.Acceptance.dll", "xml"))
    .In("."))

.Run();

string GetTestResultsPath(string directory, string assembly, string extension)
{
    return Path.Combine(
        directory,
        string.Concat(
            Path.GetFileNameWithoutExtension(assembly),
            ".TestResults.",
            extension));
}
