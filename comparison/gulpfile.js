// Use scriptcs to ensure NuGet and xunit.net executables are present 
// 1. install scriptcs: http://chocolatey.org/packages/ScriptCs
// 2. install packages: scriptcs -install

// Next, to build with gulp: 
// 1. install node:           http://chocolatey.org/packages/nodejs.install
// 2. install gulp globally:  npm install -g gulp
// 3. install modules:        npm install
// 4. execute gulpfile:       gulp

'option strict;'

var gulp = require('gulp');
var path = require('path');
var exec = require('child_process').exec;

var msBuildCommand = path.join(process.env.WINDIR, "Microsoft.NET/Framework/v4.0.30319/MSBuild.exe");
var nugetCommand = 'packages/NuGet.CommandLine.2.8.2/tools/NuGet.exe';
var xunitCommand = 'packages/xunit.runners.1.9.2/tools/xunit.console.clr4.exe';
var solution = '../src/Bau.sln';
var test = '../src/test/Bau.Test.Component/bin/Release/Bau.Test.Component.dll';

gulp.task('clean', function(cb) {
  exec(msBuildCommand + ' ' + solution + ' /target:Clean /property:Configuration=Release /verbosity:minimal /nologo', function (err, stdout, stderr) { output(err, stdout, stderr, cb); });
});

gulp.task('restore', function(cb) {
  exec(path.join(__dirname, nugetCommand) + ' restore ' + solution, function (err, stdout, stderr) { output(err, stdout, stderr, cb); });
});

gulp.task('build', ['clean', 'restore'], function(cb) {
  exec(msBuildCommand + ' ' + solution + ' /target:Build /property:Configuration=Release /verbosity:minimal /nologo', function (err, stdout, stderr) { output(err, stdout, stderr, cb); });
});

gulp.task('test', ['build'], function(cb) {
  exec(path.join(__dirname, xunitCommand) + ' ' + test + ' /html ' + test + '.TestResults.html /xml ' + test + '.TestResults.xml', function (err, stdout, stderr) { output(err, stdout, stderr, cb); });
});

gulp.task('default', ['test']);

var output = function (err, stdout, stderr, cb) {
  if (stdout) {
    console.log(stdout);
  }

  if (stderr) {
    console.log(stderr);
  }
  
  cb(err);
};