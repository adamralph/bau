// <copyright file="XunitFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauMSBuild.Test.Unit
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using FluentAssertions;
    using Xunit;

    public static class XunitFacts
    {
        [Fact]
        public static void UsesExe()
        {
            // arrange
            var task = (Derived)new Derived().Use("Foo.exe");

            // act
            var info = task.CreateStartInfo();

            // assert
            info.FileName.Should().Be(task.Exe);
        }

        [Fact]
        public static void UsesMono()
        {
            // arrange
            var task = (Derived)new Derived().Use("Foo.exe");

            // act
            var info = task.CreateStartInfo("foo.dll", true);

            // assert
            info.FileName.Should().Be("mono");
            info.Arguments.Should().Be("Foo.exe foo.dll");
        }

        [Fact]
        public static void SetsWorkingDirectory()
        {
            // arrange
            var task = (Derived)new Derived().In("foo");

            // act
            var info = task.CreateStartInfo();

            // assert
            info.WorkingDirectory.Should().Be(task.WorkingDirectory);
        }

        [Fact]
        public static void DoesNotUseShellExecute()
        {
            // arrange
            var task = new Derived();

            // act
            var info = task.CreateStartInfo();

            // assert
            info.UseShellExecute.Should().BeFalse();
        }

        [Fact]
        public static void RunsAssemblies()
        {
            // arrange
            var task = new Derived();
            var args = "foo.dll";

            // act
            var info = task.CreateStartInfo("foo.dll");

            // assert
            info.Arguments.Should().Be(args);
        }

        [Fact]
        public static void Silences()
        {
            // arrange
            var task = (Derived)new Derived().Silence();
            var option = "/silent";

            // act
            var options = task.CreateOptions().ToArray();

            // assert
            options.Length.Should().Be(1);
            options.Single().Should().Be(option);
        }

        [Fact]
        public static void Unsilences()
        {
            // arrange
            var task = (Derived)new Derived().Silence().Unsilence();

            // act
            var options = task.CreateOptions().ToArray();

            // assert
            options.Should().BeEmpty();
        }

        [Fact]
        public static void ForcesTeamCity()
        {
            // arrange
            var task = (Derived)new Derived().TeamCity();
            var option = "/teamcity";

            // act
            var options = task.CreateOptions().ToArray();

            // assert
            options.Length.Should().Be(1);
            options.Single().Should().Be(option);
        }

        [Fact]
        public static void WaitsForInput()
        {
            // arrange
            var task = (Derived)new Derived().WaitForInput();
            var option = "/wait";

            // act
            var options = task.CreateOptions().ToArray();

            // assert
            options.Length.Should().Be(1);
            options.Single().Should().Be(option);
        }

        [Fact]
        public static void DoesNotWaitForInput()
        {
            // arrange
            var task = (Derived)new Derived().DoNotWaitForInput();

            // act
            var options = task.CreateOptions().ToArray();

            // assert
            options.Should().BeEmpty();
        }

        [Fact]
        public static void DoesNotShadowCopy()
        {
            // arrange
            var task = (Derived)new Derived().NoShadow();
            var option = "/noshadow";

            // act
            var options = task.CreateOptions().ToArray();

            // assert
            options.Length.Should().Be(1);
            options.Single().Should().Be(option);
        }

        [Fact]
        public static void ShadowCopies()
        {
            // arrange
            var task = (Derived)new Derived().Shadow();

            // act
            var options = task.CreateOptions().ToArray();

            // assert
            options.Should().BeEmpty();
        }

        [Fact]
        public static void OutputsXml()
        {
            // arrange
            var task = (Derived)new Derived().Xml("{0}.xml");

            // act
            var info = task.CreateStartInfo("foo.dll");

            // assert
            info.Arguments.Should().Be("foo.dll /xml foo.dll.xml");
        }

        [Fact]
        public static void OutputsHtml()
        {
            // arrange
            var task = (Derived)new Derived().Html("{0}.html");

            // act
            var info = task.CreateStartInfo("foo.dll");

            // assert
            info.Arguments.Should().Be("foo.dll /html foo.dll.html");
        }

        [Fact]
        public static void OutputsNunit()
        {
            // arrange
            var task = (Derived)new Derived().Nunit("{0}.nunit.xml");

            // act
            var info = task.CreateStartInfo("foo.dll");

            // assert
            info.Arguments.Should().Be("foo.dll /nunit foo.dll.nunit.xml");
        }

        [Fact]
        public static void OutputsDefaultNamedXml()
        {
            // arrange
            var task = (Derived)new Derived().Xml();

            // act
            var info = task.CreateStartInfo("foo.dll");

            // assert
            info.Arguments.Should().Be("foo.dll /xml foo.dll.TestResults.xml");
        }

        [Fact]
        public static void OutputsDefaultNamedHtml()
        {
            // arrange
            var task = (Derived)new Derived().Html();

            // act
            var info = task.CreateStartInfo("foo.dll");

            // assert
            info.Arguments.Should().Be("foo.dll /html foo.dll.TestResults.html");
        }

        [Fact]
        public static void OutputsDefaultNamedNunit()
        {
            // arrange
            var task = (Derived)new Derived().Nunit();

            // act
            var info = task.CreateStartInfo("foo.dll");

            // assert
            info.Arguments.Should().Be("foo.dll /nunit foo.dll.TestResults.NUnit.xml");
        }

        [Fact]
        public static void AddArgs()
        {
            // arrange
            var task = (Derived)new Derived().With("/some /undocumented /args");
            var option = "/some /undocumented /args";

            // act
            var options = task.CreateOptions().ToArray();

            // assert
            options.Length.Should().Be(1);
            options.Single().Should().Be(option);
        }

        private class Derived : BauXunit.Xunit
        {
            public Derived()
            {
                this.Run("1.dll", "2.dll");
            }

            public new IEnumerable<string> CreateOptions()
            {
                return base.CreateOptions();
            }

            public ProcessStartInfo CreateStartInfo(string assembly)
            {
                return base.CreateStartInfo(assembly, this.CreateOptions(), false);
            }

            public ProcessStartInfo CreateStartInfo()
            {
                return base.CreateStartInfo(this.Assemblies.FirstOrDefault(), this.CreateOptions(), false);
            }

            public ProcessStartInfo CreateStartInfo(string assembly, bool isMono)
            {
                return base.CreateStartInfo(assembly, this.CreateOptions(), isMono);
            }
        }
    }
}
