// <copyright file="IBauPack{TTask}.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;

    public interface IBauPack<TTask> : IBauPack where TTask : Task, new()
    {
        new IBauPack<TTask> DependsOn(params string[] otherTasks);

        IBauPack<TTask> Do(Action<TTask> action);
    }
}
