![Bau](https://raw.githubusercontent.com/bau-build/bau/dev/assets/bau.128.png)

The C# task runner.

[![Build Status](http://teamcity.codebetter.com/app/rest/builds/buildType:%28id:bt1253%29/statusIcon)](http://teamcity.codebetter.com/viewType.html?buildTypeId=bt1253&guest=1) [![Gitter chat](https://badges.gitter.im/bau-build/bau.png)](https://gitter.im/bau-build/bau)

Bau is a community driven, cross platform, pluggable task runner built on the [scriptcs](https://github.com/scriptcs/scriptcs) + [NuGet](https://www.nuget.org/) ecosystem.

The core Bau library is packaged as a [script pack](https://github.com/scriptcs/scriptcs/wiki/Script-Packs) and provides task definition, dependencies between tasks and task running.

Extensions are provided by [plugins](https://github.com/bau-build/bau/wiki/Plugins), taking advantage of Bau's modular, pluggable architecture.

##### Task definition
```C#
// baufile.csx
Require<Bau>().Do(() => Console.WriteLine("Hello world!")).Execute();
```

##### Dependencies between tasks
```C#
// baufile.csx
string message;

Require<Bau>()
	.DependsOn("world")
	.Do(() => Console.WriteLine(message))
.Task("world")
	.DependsOn("hello")
	.Do(() => message += " world!")
.Task("hello")
	.Do(() => message = "Hello")
.Execute();
```
Tasks can be defined in any order, can depend on any number of other tasks `DependsOn("foo", "bar")`, can be referenced multiple times with multiple calls to `Task("baz")` and can have multiple actions assigned to them with multiple calls to `Do()`.

##### Running tasks
```batch
scriptcs baufile.csx
```

- [Quickstart](https://github.com/bau-build/bau/wiki/Quickstart)
- [Wiki](https://github.com/bau-build/bau/wiki)
- [Samples](https://github.com/bau-build/bau/tree/dev/src/samples)
- [NuGet package](https://nuget.org/packages/Bau/ "Bau on Nuget")
- [JabbR chat room](http://jabbr.net/#/rooms/bau)

Powered by [scriptcs](https://github.com/scriptcs/scriptcs) and [Roslyn](http://msdn.microsoft.com/en-gb/roslyn).

## Sponsors ##
Our build server is kindly provided by [CodeBetter](http://codebetter.com/) and [JetBrains](http://www.jetbrains.com/).

![YouTrack and TeamCity](http://www.jetbrains.com/img/banners/Codebetter300x250.png)
## Where can I get it

Bau is available as a [NuGet package](https://nuget.org/packages/Bau/). For update notifications, follow [@baubuild](https://twitter.com/#!/baubuild).

To build manually, clone or fork this repository and see ['How to build'](https://github.com/bau-build/bau/blob/dev/how_to_build.md).

## Can I help to improve it and/or fix bugs? ##

Absolutely! Please feel free to raise issues, fork the source code, send pull requests, etc.

No pull request is too small. Even whitespace fixes are appreciated. Before you contribute anything make sure you read [CONTRIBUTING.md](https://github.com/bau-build/bau/blob/dev/CONTRIBUTING.md).

Come and chat to fellow users and developers at the [Bau JabbR chat room](http://jabbr.net/#/rooms/bau) or [![Gitter chat](https://badges.gitter.im/bau-build/bau.png)](https://gitter.im/bau-build/bau).

## What do the version numbers mean? ##

Bau uses [Semantic Versioning](http://semver.org/). The current release is 0.x which means 'initial development'. Version 1.0 will follow the release of [scriptcs](https://github.com/scriptcs/scriptcs) version 1.0.

----------
Bau logo designed by Vanja Pakaski.