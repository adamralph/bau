// Use scriptcs to ensure NuGet and xunit.net executables are present 
// 1. install scriptcs: http://chocolatey.org/packages/ScriptCs
// 2. install packages: scriptcs -install

// Next, to build with Grunt:
// 1. install node:       http://chocolatey.org/packages/nodejs.install
// 2. install grunt-cli:  npm install -g grunt-cli
// 3. install modules:    npm install
// 4. execute gruntfile:  grunt

module.exports = function(grunt) {
  'option strict;'

  var path = require('path');

  var nugetCommand = 'scriptcs_packages/NuGet.CommandLine.3.3.0/tools/NuGet.exe';
  var xunitCommand = 'scriptcs_packages/xunit.runners.1.9.2/tools/xunit.console.clr4.exe';
  var solution = '../src/Bau.sln';
  var test = '../src/test/Bau.Test.Component/bin/Release/Bau.Test.Component.dll';

  grunt.initConfig({

    msbuild: {
      clean: {
        src: [solution],
        options: {
          targets: ['Clean'],
          projectConfiguration: 'Release',
          verbosity: 'minimal',
          stdout: true,
          version: 4.0,
          nologo: true
        }
      },

      build: {
        src: [solution],
        options: {
          targets: ['Build'],
          projectConfiguration: 'Release',
          verbosity: 'minimal',
          stdout: true,
          version: 4.0,
          nologo: true
        }
      }
    },

    exec: {

      restore: {
        command: path.join(__dirname, nugetCommand) + ' restore ' + solution,
      },

      test: {
        command: path.join(__dirname, xunitCommand) + ' ' + test + ' /html ' + test + '.TestResults.html /xml ' + test + '.TestResults.xml',
      }
    },
  });

  grunt.loadNpmTasks('grunt-msbuild');
  grunt.loadNpmTasks('grunt-exec');

  grunt.registerTask('clean',   [                     'msbuild:clean']);
  grunt.registerTask('restore', [                     'exec:restore']);
  grunt.registerTask('build',   ['clean', 'restore',  'msbuild:build']);
  grunt.registerTask('test',    ['build',             'exec:test']);
  grunt.registerTask('default', ['test']);
};
