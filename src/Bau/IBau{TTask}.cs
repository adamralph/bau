// <copyright file="IBau{TTask}.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public interface IBau<out TTask> : IBau where TTask : Task, new()
    {
        new IBau<TTask> DependsOn(params string[] otherTasks);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Do", Justification = "By design.")]
        IBau<TTask> Do(Action<TTask> action);
    }
}
