// <copyright file="ITaskBuilder.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public interface ITaskBuilder
    {
        Task CurrentTask { get; }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Fluent API.")]
        ITaskBuilder Intern<TTask>(string name = BauPack.DefaultTask) where TTask : Task, new();

        ITaskBuilder DependsOn(params string[] otherTasks);

        ITaskBuilder Do(Action action);

        void Execute();
    }
}
