// <copyright file="IBau{TTask}.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;

    public interface IBau<TTask> : IBau where TTask : Task, new()
    {
        new IBau<TTask> DependsOn(params string[] otherTasks);

        IBau<TTask> Do(Action<TTask> action);
    }
}
