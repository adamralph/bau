# 1. install scriptcs: http://chocolatey.org/packages/ScriptCs
# 2. install packages: scriptcs -install
# 3. install ruby: http://chocolatey.org/packages/ruby
# 4. install albacore: gem install albacore
# 5. execute rakefile: rake

xunit_command = "packages/xunit.runners.1.9.2/tools/xunit.console.clr4.exe"
nuget_command = "packages/NuGet.CommandLine.2.8.1/tools/NuGet.exe";
solution = "../src/Bau.sln"
component = "../src/test/Bau.Test.Component/bin/Release/Bau.Test.Component.dll";
acceptance = "../src/test/Bau.Test.Acceptance/bin/Release/Bau.Test.Acceptance.dll";

require 'albacore'

task :default => [ :component, :accept ]

msbuild :clean do |msb|
  msb.properties = { :configuration => :Release }
  msb.targets = [ :Clean ]
  msb.solution = solution
end

exec :restore do |cmd|
  cmd.command = nuget_command
  cmd.parameters "restore #{solution}"
end

msbuild :build => [:clean, :restore] do |msb|
  msb.properties = { :configuration => :Release }
  msb.targets = [ :Build ]
  msb.solution = solution
end

xunit :component => [:build] do |xunit|
  xunit.command = xunit_command
  xunit.assembly = component
  xunit.options "/html", component + ".TestResults.html", "/xml", component + ".TestResults.xml"
end

xunit :accept => [:build] do |xunit|
  xunit.command = xunit_command
  xunit.assembly = acceptance
  xunit.options "/html", acceptance + ".TestResults.html", "/xml", acceptance + ".TestResults.xml"
end
