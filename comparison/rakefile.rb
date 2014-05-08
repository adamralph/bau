# Build with Bau at least once before building with other runners to ensure NuGet and xunit.net executables are present
# 1. install ruby:      http://chocolatey.org/packages/ruby
# 2. install albacore:  gem install albacore
# 3. execute rakefile:  rake


xunit_command = "packages/xunit.runners.1.9.2/tools/xunit.console.clr4.exe"
nuget_command = "packages/NuGet.CommandLine.2.8.1/tools/NuGet.exe";
solution = "../src/Bau.sln"
test = "../src/test/Bau.Test.Component/bin/Release/Bau.Test.Component.dll";

require 'albacore'

task :default => [:test]

msbuild :clean do |msb|
  msb.properties = { :configuration => :Release }
  msb.targets = [:Clean]
  msb.solution = solution
end

exec :restore do |cmd|
  cmd.command = nuget_command
  cmd.parameters "restore #{solution}"
end

msbuild :build => [:clean, :restore] do |msb|
  msb.properties = { :configuration => :Release }
  msb.targets = [:Build]
  msb.solution = solution
end

xunit :test => [:build] do |xunit|
  xunit.command = xunit_command
  xunit.assembly = test
  xunit.options "/html", test + ".TestResults.html", "/xml", test + ".TestResults.xml"
end
