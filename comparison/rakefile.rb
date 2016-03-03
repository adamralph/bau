# Use scriptcs to ensure NuGet and xunit.net executables are present 
# 1. install scriptcs: http://chocolatey.org/packages/ScriptCs
# 2. install packages: scriptcs -install

# Next, to build with Rake:
# 1. install ruby:      http://chocolatey.org/packages/ruby
# 2. install bundler:   gem install bundler
# 3. install gems:      bundle install
# 4. execute rakefile:  bundle exec rake

require 'albacore'

nuget_command = "scriptcs_packages/NuGet.CommandLine.3.3.0/tools/NuGet.exe";
xunit_command = "scriptcs_packages/xunit.runners.1.9.2/tools/xunit.console.clr4.exe"
solution = "../src/Bau.sln"
test = "../src/test/Bau.Test.Component/bin/Release/Bau.Test.Component.dll";

task :default => [:test]

msbuild :clean do |msb|
  msb.solution = solution
  msb.targets = [:Clean]
  msb.properties = { :configuration => :Release }
  msb.verbosity = "minimal"
  msb.parameters = "/nologo"
end

exec :restore do |cmd|
  cmd.command = nuget_command
  cmd.parameters "restore #{solution}"
end

msbuild :build => [:clean, :restore] do |msb|
  msb.solution = solution
  msb.targets = [:Build]
  msb.properties = { :configuration => :Release }
  msb.verbosity = "minimal"
  msb.parameters = "/nologo"
end

xunit :test => [:build] do |xunit|
  xunit.command = xunit_command
  xunit.assembly = test
  xunit.options "/html", test + ".TestResults.html", "/xml", test + ".TestResults.xml"
end
