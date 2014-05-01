// <copyright file="ITaskBuilder{TTask}.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;

    public interface ITaskBuilder<TTask> : ITaskBuilder where TTask : Task, new()
    {
        new ITaskBuilder<TTask> DependsOn(params string[] otherTasks);

        ITaskBuilder<TTask> Do(Action<TTask> action);
    }
}
