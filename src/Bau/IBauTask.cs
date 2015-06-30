// <copyright file="IBauTask.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;
    using System.Collections.Generic;

    public interface IBauTask
    {
        string Name { get; set; }

        IList<string> Dependencies { get; }

        string Description { get; set; }

        IList<Action> Actions { get; }

        bool Invoked { get; set; }

        void Execute();
    }
}
