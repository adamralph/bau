// <copyright file="BauPack{TTask}.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;

    public class BauPack<TTask> : IBauPack<TTask> where TTask : Task, new()
    {
        private readonly IBauPack bau;

        public BauPack(IBauPack bau, string name = BauPack.DefaultTask)
        {
            Guard.AgainstNullArgument("bau", bau);

            this.bau = bau;
            this.bau.Intern<TTask>(name);
        }

        public Task CurrentTask
        {
            get { return this.bau.CurrentTask; }
        }

        public IBauPack<TTask> DependsOn(params string[] otherTasks)
        {
            this.bau.DependsOn(otherTasks);
            return this;
        }

        public IBauPack<TTask> Do(Action<TTask> action)
        {
            var task = (TTask)this.bau.CurrentTask;
            this.bau.Do(() => action(task));
            return this;
        }

        public void Execute()
        {
            this.bau.Execute();
        }

        public IBauPack Intern<UTask>(string name = BauPack.DefaultTask) where UTask : Task, new()
        {
            return this.bau.Intern<UTask>(name);
        }

        IBauPack IBauPack.DependsOn(params string[] otherTasks)
        {
            return this.bau.DependsOn(otherTasks);
        }

        public IBauPack Do(Action action)
        {
            return this.bau.Do(action);
        }
    }
}
