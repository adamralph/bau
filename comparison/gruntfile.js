// Build with Bau at least once before building with other runners to ensure NuGet and xunit.net executables are present
// 1. install node:       http://chocolatey.org/packages/nodejs.install
// 2. install grunt-cli:  npm install -g grunt-cli
// 3. install modules:    npm install
// 4. execute gruntfile:  grunt

var nugetCommand = 'packages/NuGet.CommandLine.2.8.1/tools/NuGet.exe';
var xunitCommand = "packages/xunit.runners.1.9.2/tools/xunit.console.clr4.exe";
var solution = '../src/Bau.sln';
var test = "../src/test/Bau.Test.Component/bin/Release/Bau.Test.Component.dll";

path = require('path');

module.exports = function(grunt) {

  grunt.initConfig({
    msbuild: {
      clean: {
        src: [solution],
        options: {
          projectConfiguration: 'Release',
          targets: ['Clean'],
          stdout: true,
          version: 4.0
        }
      },
      build: {
        src: [solution],
        options: {
          projectConfiguration: 'Release',
          targets: ['Build'],
          stdout: true,
          version: 4.0
        }
      }
    },
    exec: {
      restore: {
        command: function() { return path.join(__dirname, nugetCommand) + ' restore ' + solution; },
      },
      test: {
        command: function() { return path.join(__dirname, xunitCommand) + ' ' + test + ' /html ' + test + '.TestResults.html /xml ' + test + '.TestResults.xml'; },
      }
    },
  });

  grunt.loadNpmTasks('grunt-msbuild');
  grunt.loadNpmTasks('grunt-exec');

  grunt.registerTask('default', ['test']);
  grunt.registerTask('clean', ['msbuild:clean']);
  grunt.registerTask('restore', ['exec:restore']);
  grunt.registerTask('build', ['clean', 'restore', 'msbuild:build']);
  grunt.registerTask('test', ['build', 'exec:test']);
};
