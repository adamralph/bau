// <copyright file="BauFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Component
{
    using BauCore;
    using BauExec;
    using Xunit;

    public static class BauFacts
    {
        [Fact]
        public static void SyntaxWorks()
        {
            new Bau()
            .Task()
                .DependsOn("task1", "task3")
                .Do(() => { })
            .Exec("task1")
                .DependsOn("task2")
                .Do(exec => exec.Run("noop.bat"))
            .Task("task2")
                .DependsOn("task3")
                .Do(() => { })
            .Exec("task3")
                .Do(exec => exec.Run("noop.bat"))
            .Execute();
        }
    }
}
