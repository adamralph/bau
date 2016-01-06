// <copyright file="IBauTask.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public interface IBauTask
    {
        string Name { get; set; }

        IList<string> Dependencies { get; }

        string Description { get; set; }

        IList<Action> Actions { get; }

        FileInfo InputFile
        {
            get;
            set;
        }

        FileInfo OutputFile
        {
            get;
            set;
        }

        bool IsUpToDate
        {
            get;
        }

        bool Invoked { get; set; }

        void Execute();
    }
}
