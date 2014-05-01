// <copyright file="TaskBuilder{TTask}.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;

    public class TaskBuilder<TTask> : ITaskBuilder<TTask> where TTask : Task, new()
    {
        private readonly ITaskBuilder builder;

        public TaskBuilder(ITaskBuilder builder)
        {
            Guard.AgainstNullArgument("builder", builder);

            this.builder = builder;
        }

        public ITaskBuilder<TTask> Intern(string name = BauPack.DefaultTask)
        {
            this.builder.Intern<TTask>(name);
            return this;
        }

        public ITaskBuilder<TTask> DependsOn(params string[] otherTasks)
        {
            this.builder.DependsOn(otherTasks);
            return this;
        }

        public ITaskBuilder<TTask> Do(Action<TTask> action)
        {
            this.builder.Do(() => action((TTask)this.builder.CurrentTask));
            return this;
        }

        public void Execute()
        {
            this.builder.Execute();
        }
    }
}
