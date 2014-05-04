// <copyright file="BauExtensions.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    public static class BauExtensions
    {
        public static IBau Task(this IBau bau, string name = Bau.DefaultTask)
        {
            Guard.AgainstNullArgument("bau", bau);

            return bau.Intern<Task>(name);
        }

        public static IBau<TTask> Task<TTask>(this IBau bau, string name = Bau.DefaultTask) where TTask : Task, new()
        {
            Guard.AgainstNullArgument("bau", bau);

            return new Bau<TTask>(bau, name);
        }
    }
}
