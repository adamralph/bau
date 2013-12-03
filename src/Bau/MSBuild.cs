// <copyright file="MSBuild.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System.Collections.Generic;

    public class MSBuild : Exec
    {
        public string Solution { get; set; }

        public string Verbosity { get; set; }

        public string Logger { get; set; }

        public int MaxCpuCount { get; set; }

        public string[] Targets { get; set; }

        public Dictionary<string, string> Properties { get; set; }

        public Dictionary<string, string> OtherSwitches { get; set; }
    }
}
