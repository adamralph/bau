// <copyright file="TaskBuilder{TTask}.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;

    public class TaskBuilder<TTask> : ITaskBuilder<TTask> where TTask : class, IBauTask, new()
    {
        private readonly ITaskBuilder builder;

        public TaskBuilder(ITaskBuilder builder, string name = null)
        {
            Guard.AgainstNullArgument("builder", builder);

            this.builder = builder;
            this.builder.Intern<TTask>(name);
        }

        public IBauTask CurrentTask
        {
            get { return this.builder.CurrentTask; }
        }

        public ITaskBuilder<TTask> DependsOn(params string[] otherTasks)
        {
            this.builder.DependsOn(otherTasks);
            return this;
        }

        public ITaskBuilder WithAliases(params string[] aliases)
        {
            this.builder.WithAliases(aliases);
            return this;
        }

        public ITaskBuilder<TTask> Desc(string description)
        {
            this.builder.Desc(description);
            return this;
        }

        public ITaskBuilder<TTask> Do(Action<TTask> action)
        {
            var task = (TTask)this.builder.CurrentTask;
            this.builder.Do(() => action(task));
            return this;
        }

        public void Run()
        {
            this.builder.Run();
        }

        [Obsolete("Use Run() instead.")]
        public void Execute()
        {
            this.Run();
        }

        public ITaskBuilder Intern<TNewTask>(string name = null) where TNewTask : class, IBauTask, new()
        {
            return this.builder.Intern<TNewTask>(name);
        }

        ITaskBuilder ITaskBuilder.DependsOn(params string[] otherTasks)
        {
            return this.builder.DependsOn(otherTasks);
        }

        ITaskBuilder ITaskBuilder.Desc(string description)
        {
            return this.builder.Desc(description);
        }

        public ITaskBuilder Do(Action action)
        {
            return this.builder.Do(action);
        }

        public void Invoke(string task)
        {
            this.builder.Invoke(task);
        }

        public void Reenable(string task)
        {
            this.builder.Reenable(task);
        }

        public void Execute(string task)
        {
            this.builder.Execute(task);
        }
    }
}
