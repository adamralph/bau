// <copyright file="ITaskBuilder.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;

    public interface ITaskBuilder
    {
        Task CurrentTask { get; }

        ITaskBuilder Intern<TTask>(string name = BauPack.DefaultTask) where TTask : Task, new();

        ITaskBuilder DependsOn(params string[] otherTasks);

        ITaskBuilder Do(Action action);

        void Execute();
    }
}
