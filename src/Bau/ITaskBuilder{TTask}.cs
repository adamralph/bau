// <copyright file="ITaskBuilder{TTask}.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public interface ITaskBuilder<out TTask> : ITaskBuilder where TTask : Task, new()
    {
        new ITaskBuilder<TTask> DependsOn(params string[] otherTasks);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Do", Justification = "By design.")]
        ITaskBuilder<TTask> Do(Action<TTask> action);
    }
}
