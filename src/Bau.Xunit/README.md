# ![Bau](https://raw.githubusercontent.com/bau-build/bau/dev/assets/bau.128.png).![xUnit.net](https://raw.github.com/xunit/media/master/full-logo.png)

> A Bau plugin for running [xUnit.net](http://xunit.net) tests

[![Build Status](http://teamcity.codebetter.com/app/rest/builds/buildType:%28id:bt1253%29/statusIcon)](http://teamcity.codebetter.com/viewType.html?buildTypeId=bt1253&guest=1) [![Gitter chat](https://badges.gitter.im/bau-build/bau.png)](https://gitter.im/bau-build/bau)

## Install

```batch
> scriptcs -install Bau.Xunit -pre
```

## Usage

### Examples

#### Fluent

```C#
// simple
bau.Xunit("tests").Do(xunit => xunit
	.UseExe("path/to/xunit.console.clr4.exe")
	.RunAssemblies("path/to/MyApp.MyAssembly.Tests.dll"));
```
```C#
// advanced
bau.Xunit("tests").Do(xunit => xunit
	.UseExe("path/to/xunit.console.clr4.exe")
	.RunAssemblies("path/to/MyApp.MyAssembly1.Tests.dll", "path/to/MyApp.MyAssembly2.Tests.dll")
	.Silence() // can be reversed with .Unsilence()
	.ForceTeamCity()
	.DoNotShadowCopy() // can be reversed with .ShadowCopy()
	.OutputXml("{0}.xml")
	.OutputHtml("{0}.html")
	.OutputNunitXml("{0}.NUnit.xml")
	.In("path/to/working/directory"));
```

#### Declarative

```C#
// simple
bau.Xunit("tests").Do(xunit =>
{
	xunit.Exe = "path/to/xunit.console.clr4.exe";
	xunit.Assemblies = new[] { "path/to/MyApp.MyAssembly.Tests.dll", };
});
```
```C#
// advanced
bau.Xunit("tests").Do(xunit =>
{
	xunit.Exe = "path/to/xunit.console.clr4.exe";
	xunit.Assemblies = new[] { "path/to/MyApp.MyAssembly.Tests.dll", "path/to/MyApp.MyAssembly2.Tests.dll", };
	xunit.Silent = true;
	xunit.TeamCity = true;
	xunit.NoShadow = true;
	xunit.XmlFormat = "{0}.xml";
	xunit.XmlFormat = "{0}.html")
	xunit.XmlFormat = "{0}.NUnit.xml")
	xunit.WorkingDirectory = "path/to/working/directory";
});
```

### API

All methods and properties shown in the examples map directly to the xUnit.net command line, with the exception of `UseExe()`/`Exe` and `In()`/`WorkingDirectory`, which are self-explanatory. For details of the xUnit.net command line, execute the xUnit.net console with the help option, e.g. `xunit.console.clr4.exe /?`.

xUnit.net project files can be used by passing their paths to `RunAssemblies()` instead of paths to assembly files.

The API also provides a `With(string args)` method and an equivalent `Args` property which can be used to pass undocumented options or are sometimes useful as terse alternatives to the methods and properties described above. The specified string is appended to the string rendered from other options.  

Trait options are not yet supported by the API but can be specified using `With(string args)` or `Args`.

```C#
// fluent
bau.Xunit("tests").Do(xunit => xunit
	.UseExe("path/to/xunit.console.clr4.exe")
	.RunAssemblies("path/to/MyApp.MyAssembly.Tests.dll")
	.With(@"/trait ""Foo=bar"" /-trait ""Foo=baz"""));
```
```C#
// declarative
bau.Xunit("tests").Do(xunit =>
{
	xunit.Exe = "path/to/xunit.console.clr4.exe";
	xunit.Assemblies = new[] { "path/to/MyApp.MyAssembly.Tests.dll", };
	xunit.Args = @"/trait ""Foo=bar"" /-trait ""Foo=baz""";
});
```
