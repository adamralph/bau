// <copyright file="Exec.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using Common.Logging;

    public class Exec : Target
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        public string Command { get; set; }

        public string[] Parameters { get; set; }

        public override void Execute()
        {
            base.Execute();
            if (this.Command == null)
            {
                var message = string.Format(CultureInfo.InvariantCulture, "'{0}' target has no command.", this.Name);
                throw new InvalidOperationException(message);
            }

            new Process().Execute(this.Command, this.Parameters);
        }
    }
}
