// <copyright file="BauPackExtensions.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    public static class BauPackExtensions
    {
        public static ITaskBuilder Task(this ITaskBuilder builder, string name = BauPack.DefaultTask)
        {
            Guard.AgainstNullArgument("builder", builder);

            return builder.Intern<Task>(name);
        }

        public static ITaskBuilder<ExecTask> Exec(this ITaskBuilder builder, string name = BauPack.DefaultTask)
        {
            return new TaskBuilder<ExecTask>(builder, name);
        }
    }
}
