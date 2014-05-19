// <copyright file="ITaskBuilder.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public interface ITaskBuilder
    {
        BauTask CurrentTask { get; }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Fluent API.")]
        ITaskBuilder Intern<TTask>(string name = Bau.DefaultTask) where TTask : BauTask, new();

        ITaskBuilder DependsOn(params string[] otherTasks);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Do", Justification = "By design.")]
        ITaskBuilder Do(Action action);

        void Run();

        [Obsolete("Use Run() instead.")]
        void Execute();

        void Invoke(string task);

        void Reenable(string task);

        void Execute(string task);
    }
}
