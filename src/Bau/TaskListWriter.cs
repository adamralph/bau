// <copyright file="TaskListWriter.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class TaskListWriter
    {
        private const string IndentationPrefix = "    ";
        private static readonly ConsoleColor DefaultColor = ConsoleColor.White;
        private static readonly ConsoleColor DependenciesColor = ConsoleColor.Gray;
        private static readonly Regex NameEscapeRequiredRegex = new Regex(@"[\s#]");

        public bool RequireDescription { get; set; }

        public bool ShowDescription { get; set; }

        public bool ShowPrerequisites { get; set; }

        public bool FormatAsJson { get; set; }

        public IEnumerable<ColorText> CreateTaskListingLines(IEnumerable<IBauTask> tasks)
        {
            return this.FormatAsJson
                ? this.CreateJsonTaskListingLines(tasks)
                : this.CreatePlainTextTaskListingLines(tasks);
        }

        private static string EscapeTaskName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return "\"\"";
            }

            if (NameEscapeRequiredRegex.IsMatch(name))
            {
                return "\"" + name + "\"";
            }

            return name;
        }

        private IEnumerable<ColorText> CreatePlainTextTaskListingLines(IEnumerable<IBauTask> allTasks)
        {
            var tasks = allTasks;

            if (this.RequireDescription)
            {
                tasks = tasks.Where(t => t.Description != null);
            }

            tasks = tasks.OrderBy(t => t.Name);

            var printableTasks = tasks
                .Select(t => new
                {
                    Description = t.Description,
                    EscapedName = EscapeTaskName(t.Name),
                    Dependencies = t.Dependencies
                })
                .ToList();

            int escapedNamePadAmmount = this.ShowDescription && printableTasks.Count > 0
                ? printableTasks.Max(t => t.EscapedName.Length)
                : 0;

            foreach (var t in printableTasks)
            {
                string fullLine;
                if (this.ShowDescription)
                {
                    fullLine = t.EscapedName.PadRight(escapedNamePadAmmount)
                        + " # "
                        + t.Description;
                }
                else
                {
                    fullLine = t.EscapedName;
                }

                yield return new ColorText(new ColorToken(fullLine, DefaultColor));

                if (this.ShowPrerequisites)
                {
                    foreach (var dependency in t.Dependencies)
                    {
                        yield return new ColorText(new ColorToken(
                            IndentationPrefix + dependency,
                            DependenciesColor));
                    }
                }
            }
        }

        private IEnumerable<ColorText> CreateJsonTaskListingLines(IEnumerable<IBauTask> tasks)
        {
            throw new NotImplementedException();
        }
    }
}
