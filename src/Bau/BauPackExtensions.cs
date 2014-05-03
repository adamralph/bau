// <copyright file="BauPackExtensions.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    public static class BauPackExtensions
    {
        public static IBauPack Task(this IBauPack bau, string name = BauPack.DefaultTask)
        {
            Guard.AgainstNullArgument("bau", bau);

            return bau.Intern<Task>(name);
        }

        public static IBauPack<TTask> Task<TTask>(this IBauPack bau, string name = BauPack.DefaultTask) where TTask : Task, new()
        {
            Guard.AgainstNullArgument("bau", bau);

            return new BauPack<TTask>(bau, name);
        }
    }
}
