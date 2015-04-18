# How to build

Bau has used itself to build itself from the very start. A practice known as 'dogfooding' :neckbeard:.

These instructions are *only* for building with Bau, including compilation, tests and packaging. This is the simplest way to build.

You can also build with Visual Studio 2012 or later but you'll have to run the tests yourself and packaging will not take place.

## Prerequisites

* (Windows) .NET framework 4.5 or later.
* (Mac/Linux) [Mono](http://www.mono-project.com/download/) 3.0 or later. (On Linux, only mono-devel is required.)
* (All platforms) [scriptcs](https://github.com/scriptcs/scriptcs/wiki/Installation) 0.13.3 or later.

### Building

Using a command prompt, navigate to your clone root folder and execute:

`build.cmd` (Windows)

`build.sh` (Mac/Linux) 

This executes the default build tasks. After the build has completed, the build artifacts will be located in `artifacts`.

To run the acceptance test task (and all dependencies), execute:

`build.cmd accept` / `build.sh accept`

To run *all* tasks, execute:

`build.cmd all` / `build.sh all`
