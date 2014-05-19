// <copyright file="IBauTask.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;
    using System.Collections.Generic;

    public interface IBauTask
    {
        IList<string> Dependencies { get; }

        IList<Action> Actions { get; }

        bool Invoked { get; set; }

        void Execute();
    }
}
