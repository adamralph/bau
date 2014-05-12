// <copyright file="Bau{TTask}.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;

    public class Bau<TTask> : IBau<TTask> where TTask : Task, new()
    {
        private readonly IBau bau;

        public Bau(IBau bau, string name = Bau.DefaultTask)
        {
            Guard.AgainstNullArgument("bau", bau);

            this.bau = bau;
            this.bau.Intern<TTask>(name);
        }

        public Task CurrentTask
        {
            get { return this.bau.CurrentTask; }
        }

        public IBau<TTask> DependsOn(params string[] otherTasks)
        {
            this.bau.DependsOn(otherTasks);
            return this;
        }

        public IBau<TTask> Do(Action<TTask> action)
        {
            var task = (TTask)this.bau.CurrentTask;
            this.bau.Do(() => action(task));
            return this;
        }

        public void Execute()
        {
            this.bau.Execute();
        }

        public IBau Intern<TNewTask>(string name = Bau.DefaultTask) where TNewTask : Task, new()
        {
            return this.bau.Intern<TNewTask>(name);
        }

        IBau IBau.DependsOn(params string[] otherTasks)
        {
            return this.bau.DependsOn(otherTasks);
        }

        public IBau Do(Action action)
        {
            return this.bau.Do(action);
        }
    }
}
