# How to build

Bau has used itself to build itself from the very start. A practice known as 'dogfooding' :neckbeard:.

These instructions are *only* for building with Bau, including compilation, tests and packaging. This is the simplest way to build.

You can also build with Visual Studio 2012 or later but you'll have to run the tests yourself and packaging will not take place.

*Don't be put off by the prerequisites!* It only takes a few minutes to set them up and only needs to be done once. If you haven't used Bau before then you're in for a real treat!

At the time of writing the build is only confirmed to work on Windows using the Microsoft .NET framework.

## Prerequisites

1. Ensure you have .NET framework 4.5 installed.

1. Ensure you have [Chocolatey](http://chocolatey.org/) installed.

1. Ensure you have [scriptcs](http://chocolatey.org/packages/ScriptCs) installed.

## Building

Using a command prompt, navigate to your clone root folder and execute:

`bau.bat`

This executes the default build tasks. After the build has completed, the build artifacts will be located in `artifacts`.
