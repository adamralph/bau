# How to build

Bau has used itself to build itself from the very start. A practice known as 'dogfooding' :neckbeard:.

These instructions are *only* for building with Bau, including compilation, tests and packaging. This is the simplest way to build.

You can also build with Visual Studio 2012 or later but you'll have to run the tests yourself and packaging will not take place.

*Don't be put off by the prerequisites!* It only takes a few minutes to set them up and only needs to be done once. If you haven't used Bau before then you're in for a real treat!

## Windows

### Prerequisites

1. Ensure you have .NET framework 4.5 installed.

1. Ensure you have [scriptcs](http://chocolatey.org/packages/ScriptCs) installed.

### Building

Using a command prompt, navigate to your clone root folder and execute:

`bau.bat`

This executes the default build tasks. After the build has completed, the build artifacts will be located in `artifacts`.

To run the acceptance tests (and all dependencies), execute:

`bau.bat accept`

To run *all* tasks, execute:

`bau.bat all`

## Linux

### Prerequisites

1. Ensure you have Mono development tools 3.0 or later installed.

	`sudo apt-get install mono-devel`

1. Ensure your mono instance has root SSL certificates

	`mozroots --import --sync`

### Building

Using a terminal, navigate to your clone root folder and execute:

`bash bau.sh`

This executes the default build tasks. After the build has completed, the build artifacts will be located in `artifacts`.
