// <copyright file="TaskListWriter.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    public class TaskListWriter
    {
        public bool RequireDescription { get; set; }

        public bool ShowDescription { get; set; }

        public bool ShowPrerequisites { get; set; }

        public bool FormatAsJson { get; set; }
    }
}
