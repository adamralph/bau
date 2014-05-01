// <copyright file="IBauPack.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public interface IBauPack
    {
        Task CurrentTask { get; }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Fluent API.")]
        IBauPack Intern<TTask>(string name = BauPack.DefaultTask) where TTask : Task, new();

        IBauPack DependsOn(params string[] otherTasks);

        IBauPack Do(Action action);

        void Execute();
    }
}
