require 'albacore'

solution = "src/ConfigR.sln"

desc "Execute default tasks"
task :default => [:build]

desc "Restore NuGet packages"
exec :restore do |cmd|
  cmd.command = nuget_command
  cmd.parameters "restore #{solution}"
end

desc "Clean solution"
msbuild :clean do |msb|
  msb.properties = { :configuration => :Release }
  msb.targets = [:Clean]
  msb.solution = solution
end

desc "Build solution"
msbuild :build => [:clean, :restore] do |msb|
  msb.properties = { :configuration => :Release }
  msb.targets = [:Build]
  msb.solution = solution
end
