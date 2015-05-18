// <copyright file="TaskListWriter.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class TaskListWriter
    {
        private const string IndentationPrefix = "    ";
        private static readonly ConsoleColor DefaultColor = ConsoleColor.White;
        private static readonly ConsoleColor DependenciesColor = ConsoleColor.Gray;
        private static readonly ConsoleColor JsonDataColor = ConsoleColor.White;
        private static readonly ConsoleColor JsonSyntaxColor = ConsoleColor.Gray;
        private static readonly Regex NameEscapeRequiredRegex = new Regex(@"[\s#]");

        public TaskListWriter(TaskListingKind listingKind)
        {
            this.ShowDescription = true;

            switch (listingKind)
            {
                case TaskListingKind.TextDescribed:
                    this.RequireDescription = true;
                    break;
                case TaskListingKind.TextPrereq:
                    this.ShowPrerequisites = true;
                    break;
                case TaskListingKind.Json:
                    this.FormatAsJson = true;
                    this.ShowPrerequisites = true;
                    break;
                case TaskListingKind.TextAll:
                    break;
                case TaskListingKind.None:
                default:
                    throw new NotSupportedException(string.Format("Task listing is not supported: {0}", listingKind));
            }
        }

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
            var printableTasks = allTasks;

            if (this.RequireDescription)
            {
                printableTasks = printableTasks.Where(t => t.Description != null);
            }

            printableTasks = printableTasks.OrderBy(t => t.Name);

            var printableTaskProperties = printableTasks
                .Select(t => new
                {
                    Description = t.Description,
                    EscapedName = EscapeTaskName(t.Name),
                    Dependencies = t.Dependencies
                })
                .ToList();

            int escapedNamePadAmmount = 0;
            if (this.ShowDescription)
            {
                foreach (var nameLength in printableTaskProperties
                    .Where(t => t.Description != null)
                    .Select(t => t.EscapedName.Length))
                {
                    if (nameLength > escapedNamePadAmmount)
                    {
                        escapedNamePadAmmount = nameLength;
                    }
                }
            }

            foreach (var t in printableTaskProperties)
            {
                string fullLine;
                if (this.ShowDescription && t.Description != null)
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

        private IEnumerable<ColorText> CreateJsonTaskListingLines(IEnumerable<IBauTask> allTasks)
        {
            // TODO: replace with a JSON serialization library when an appropriate dependency is taken
            yield return new ColorText(new ColorToken("{", JsonSyntaxColor));
            yield return new ColorText(new ColorToken(IndentationPrefix + "\"tasks\": [", JsonSyntaxColor));

            var indent2 = IndentationPrefix + IndentationPrefix;
            var indent3 = indent2 + IndentationPrefix;
            var indent4 = indent3 + IndentationPrefix;

            var printableTasks = allTasks.OrderBy(t => t.Name).ToList();
            var lastTaskIndex = printableTasks.Count - 1;
            for (var taskIndex = 0; taskIndex <= lastTaskIndex; ++taskIndex)
            {
                var task = printableTasks[taskIndex];

                yield return new ColorText(new ColorToken(indent2 + "{", JsonSyntaxColor));

                yield return new ColorText(new ColorToken(
                    indent3 + "\"name\": " + this.CreateJsonString(task.Name) + (this.ShowDescription || this.ShowPrerequisites ? "," : string.Empty),
                    JsonDataColor));

                if (this.ShowDescription)
                {
                    yield return new ColorText(new ColorToken(
                        indent3 + "\"description\": " + this.CreateJsonString(task.Description) + (this.ShowPrerequisites ? "," : string.Empty),
                        JsonDataColor));
                }

                if (this.ShowPrerequisites)
                {
                    yield return new ColorText(
                        new ColorToken(indent3 + "\"dependencies\":", JsonDataColor),
                        new ColorToken(" [", JsonSyntaxColor));

                    if (task.Dependencies != null)
                    {
                        var lastDependencyIndex = task.Dependencies.Count - 1;
                        for (int dependencyIndex = 0; dependencyIndex <= lastDependencyIndex; ++dependencyIndex)
                        {
                            var jsonStringValue = this.CreateJsonString(task.Dependencies[dependencyIndex]);
                            if (dependencyIndex != lastDependencyIndex)
                            {
                                jsonStringValue += ",";
                            }

                            yield return new ColorText(
                                new ColorToken(indent4 + jsonStringValue, JsonDataColor));
                        }
                    }

                    yield return new ColorText(
                        new ColorToken(indent3 + "]", JsonSyntaxColor));
                }
                
                yield return new ColorText(new ColorToken(
                    indent2 + (taskIndex == lastTaskIndex ? "}" : "},"),
                    JsonSyntaxColor));
            }

            yield return new ColorText(new ColorToken(IndentationPrefix + "]", JsonSyntaxColor));
            yield return new ColorText(new ColorToken("}", JsonSyntaxColor));
        }
        
        private string CreateJsonString(string value)
        {
            // TODO: remove this method when Bau takes a dependency on a JSON library
            // NOTE: this method is probably not very good
            if (value == null)
            {
                return "null";
            }

            var builder = new StringBuilder();

            builder.Append('\"');

            foreach (var c in value)
            {
                if (c == '\"')
                {
                    builder.Append(@"\""");
                }
                else if (c == '\\')
                {
                    builder.Append(@"\\");
                }
                else if (c == '\b')
                {
                    builder.Append(@"\b");
                }
                else if (c == '\f')
                {
                    builder.Append(@"\f");
                }
                else if (c == '\n')
                {
                    builder.Append(@"\n");
                }
                else if (c == '\r')
                {
                    builder.Append(@"\r");
                }
                else if (c == '\t')
                {
                    builder.Append(@"\t");
                }
                else if (char.IsControl(c))
                {
                    builder.Append(@"\u");
                    builder.Append(checked((ushort)c).ToString("X4"));
                }
                else
                {
                    builder.Append(c);
                }
            }

            builder.Append('\"');

            return builder.ToString();
        }
    }
}
