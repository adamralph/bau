// Use scriptcs to ensure NuGet and xunit.net executables are present 
// 1. install scriptcs: http://chocolatey.org/packages/ScriptCs
// 2. install packages: scriptcs -install

// Next, to build with gulp: 
// 1. install node:       http://chocolatey.org/packages/nodejs.install
// 2. install grunt-cli:  npm install -g gulp
// 3. install modules:    npm install
// 4. execute gruntfile:  gulp

// The callbacks are passed in to provide a hint to gulp that these tasks need to run synchronously. I had looked at simply returning the pipe as well
// so that the tasks would simple chain. However, that didn't work for me.

// I believe this is fairly idiomatic for a gulp impl but I'd prefer someone with more knowledge provide insight.

var nugetCommand = 'packages/NuGet.CommandLine.2.8.1/tools/NuGet.exe';
var xunitCommand = 'packages/xunit.runners.1.9.2/tools/xunit.console.clr4.exe';
var solution = '../src/Bau.sln';
var test = '../src/test/Bau.Test.Component/bin/Release/Bau.Test.Component.dll';

var gulp = require('gulp');
var msbuild = require('gulp-msbuild');
var shell = require('gulp-shell')

path = require('path');

gulp.task('clean', function(cb) {
    gulp.src(solution)
        .pipe(msbuild({
            targets: ['Clean'],
            })
        );
	cb();
});

gulp.task('test', ['build'], shell.task([path.join(__dirname, xunitCommand) + ' ' + test + ' /html ' + test + '.TestResults.html /xml ' + test + '.TestResults.xml']));

gulp.task('restore', shell.task(path.join(__dirname, nugetCommand) + ' restore ' + solution));

gulp.task('build', ['restore'], function(cb) {
    gulp.src(solution)
        .pipe(msbuild({
			configuration: 'Release'
            })
        );
	cb();
});

gulp.task('default', ['test']);