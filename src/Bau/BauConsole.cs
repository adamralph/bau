// <copyright file="BauConsole.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;
    using System.Linq;
    using System.Reflection;

    internal static class BauConsole
    {
        public static void WriteHeader()
        {
            var version = (AssemblyInformationalVersionAttribute)Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute)).Single();

            using (new LineWriter(ConsoleColor.Gray))
            {
                using (new ConsoleColorizer(ConsoleColor.White))
                {
                    Console.Write("Bau");
                }

                Console.Write(" {0}", version.InformationalVersion);

                using (new ConsoleColorizer(ConsoleColor.DarkGray))
                {
                    Console.Write(" Copyright (c) Bau contributors (baubuildch@gmail.com)");
                }
            }
        }

        public static void WriteExecuteDeprecated()
        {
            using (new LineWriter(ConsoleColor.Yellow))
            {
                Console.Write("Bau.Execute() (with no parameters) will be removed shortly. Use Bau.Run() instead.");
            }
        }

        public static void WriteInvalidTaskName(string task)
        {
            using (new LineWriter(ConsoleColor.Red))
            {
                Console.Write("Invalid task name ");
                WriteTask(task);
            }
        }

        public static void WriteTasksAlreadyExists(string task, string type)
        {
            using (new LineWriter(ConsoleColor.Red))
            {
                WriteTask(task);
                Console.Write(" already exists with type '");
                using (new ConsoleColorizer(ConsoleColor.DarkMagenta))
                {
                    Console.Write(type);
                }

                Console.Write("'");
            }
        }

        public static void WriteTaskNotFound(string task)
        {
            using (new LineWriter(ConsoleColor.Red))
            {
                WriteTask(task);
                Console.Write(" task not found");
            }
        }

        public static void WriteTaskStarting(string task)
        {
            using (new LineWriter(ConsoleColor.Gray))
            {
                Console.Write("Starting ");
                WriteTask(task);
                Console.Write("...");
            }
        }

        public static void WriteTaskFinished(string task, double milliseconds)
        {
            using (new LineWriter(ConsoleColor.Gray))
            {
                Console.Write("Finished ");
                WriteTask(task);
                Console.Write(" after ");
                WriteMilliseconds(milliseconds);
            }
        }

        public static void WriteTaskFailed(string task, double milliseconds, string exceptionMessage)
        {
            using (new LineWriter(ConsoleColor.Gray))
            {
                WriteTask(task);
                Console.Write(" failed after ");
                WriteMilliseconds(milliseconds);
                Console.Write(" ");
                using (new ConsoleColorizer(ConsoleColor.DarkRed))
                {
                    Console.Write(exceptionMessage);
                }
            }
        }

        public static void WriteTaskMessage(string task, string message, ConsoleColor color)
        {
            using (new TaskWriter(task, color))
            {
                Console.Write(message);
            }
        }

        private static void WriteTask(string task)
        {
            Console.Write("'");
            WriteTaskName(task);
            Console.Write("'");
        }

        private static void WriteTaskName(string task)
        {
            using (new ConsoleColorizer(ConsoleColor.DarkCyan))
            {
                Console.Write(task);
            }
        }

        private static void WriteMilliseconds(double milliseconds)
        {
            using (new ConsoleColorizer(ConsoleColor.DarkYellow))
            {
                Console.Write(milliseconds.ToStringFromMilliseconds());
            }
        }

        private sealed class TaskWriter : LineWriter
        {
            public TaskWriter(string task, ConsoleColor foregroundColor)
                : base(foregroundColor)
            {
                using (new ConsoleColorizer(ConsoleColor.Gray))
                {
                    Console.Write("[");
                    BauConsole.WriteTaskName(task);
                    Console.Write("] ");
                }
            }
        }

        private class LineWriter : ConsoleColorizer
        {
            public LineWriter(ConsoleColor foregroundColor)
                : base(foregroundColor)
            {
                using (new ConsoleColorizer(ConsoleColor.Gray))
                {
                    Console.Write("[");
                    using (new ConsoleColorizer(ConsoleColor.DarkGreen))
                    {
                        Console.Write("Bau");
                    }

                    Console.Write("] ");
                }
            }

            public override void Dispose()
            {
                Console.WriteLine();
                base.Dispose();
            }
        }

        // NOTE (adamralph): must remain private until Dipose() is implemented properly
        private class ConsoleColorizer : IDisposable
        {
            private readonly ConsoleColor? originalForegroundColor;

            public ConsoleColorizer(ConsoleColor foregroundColor)
            {
                this.originalForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = foregroundColor;
            }

            public virtual void Dispose()
            {
                if (this.originalForegroundColor.HasValue)
                {
                    Console.ForegroundColor = this.originalForegroundColor.Value;
                }
            }
        }
    }
}
