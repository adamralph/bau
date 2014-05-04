// <copyright file="IBau.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public interface IBau
    {
        Task CurrentTask { get; }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Fluent API.")]
        IBau Intern<TTask>(string name = Bau.DefaultTask) where TTask : Task, new();

        IBau DependsOn(params string[] otherTasks);

        IBau Do(Action action);

        void Execute();
    }
}
