// <copyright file="BauExtensions.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    public static class BauExtensions
    {
        public static ITaskBuilder Task(this ITaskBuilder builder, string name = null)
        {
            Guard.AgainstNullArgument("builder", builder);

            return builder.Intern<BauTask>(name);
        }

        public static ITaskBuilder<TTask> Task<TTask>(this ITaskBuilder builder, string name = null)
            where TTask : class, IBauTask, new()
        {
            Guard.AgainstNullArgument("builder", builder);

            return new TaskBuilder<TTask>(builder, name);
        }
    }
}
