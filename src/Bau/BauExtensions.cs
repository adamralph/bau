// <copyright file="BauExtensions.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    public static class BauExtensions
    {
        public static ITaskBuilder Task(this ITaskBuilder builder, string name = Bau.DefaultTask)
        {
            Guard.AgainstNullArgument("builder", builder);

            return builder.Intern<BauTask>(name);
        }

        public static ITaskBuilder<TTask> Task<TTask>(this ITaskBuilder builder, string name = Bau.DefaultTask) where TTask : BauTask, new()
        {
            Guard.AgainstNullArgument("builder", builder);

            return new TaskBuilder<TTask>(builder, name);
        }
    }
}
