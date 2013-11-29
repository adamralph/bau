

var solution = "src/ConfigR.sln";

Desc("Execute default tasks");
Task("default", new[] { "build" });

Desc("Restore NuGet packages");
Exec("restore", cmd => {
  cmd.command = nuget_command;
  cmd.parameters = "restore " + solution;
});

Desc("Clean solution");
MSBuild("clean", msb => {
  msb.Properties = new { Configuration = "Release" };
  msb.Targets = new [] { "Clean" };
  msb.Solution = solution;
});

Desc("Build solution");
MSBuild("build", new[] { "clean", "restore" }, msb => {
  msb.Properties = new { Configuration = "Release" };
  msb.Targets = new [] { "Build" };
  msb.Solution = solution;
});
