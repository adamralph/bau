// <copyright file="BauPackFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Component
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    public static class BauPackFacts
    {
        [Fact]
        public static void SyntaxWorks()
        {
            new BauPack()
            .Task()
                .DependsOn("task1", "task3")
                .Do(() => { })
            .Exec("task1")
                .DependsOn("task2")
                .Do(exec => exec.Command = "ipconfig")
            .Task("task2")
                .DependsOn("task3")
                .Do(() => { })
            .Exec("task3")
                .Do(exec => exec.Command = "ipconfig")
            .Execute();
        }
    }
}
