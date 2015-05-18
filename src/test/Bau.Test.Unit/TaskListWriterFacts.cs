// <copyright file="TaskListWriterFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Xunit;
    using Xunit.Extensions;

    public class TaskListWriterFacts
    {
        protected static BauTask[] CreateSampleTasks()
        {
            return new[]
            {
                new BauTask
                {
                    Name = "default",
                    Description = "Execute default tasks",
                    Dependencies =
                    {
                        "component",
                        "accept",
                        "pack"
                    }
                },
                new BauTask
                {
                    Name = "restore",
                    Description = "Restore NuGet packages",
                },
                new BauTask
                {
                    Name = "artifacts/logs"
                },
                new BauTask
                {
                    Name = "clean",
                    Description = "Clean solution",
                    Dependencies =
                    {
                        "artifacts/logs"
                    }
                },
                new BauTask
                {
                    Name = "build",
                    Description = "Build solution",
                    Dependencies =
                    {
                        "clean",
                        "restore",
                        "artifacts/logs"
                    }
                },
                new BauTask
                {
                    Name = "component",
                    Description = "Run component tests",
                    Dependencies =
                    {
                        "build"
                    }
                },
                new BauTask
                {
                    Name = "accept",
                    Description = "Run acceptance tests",
                    Dependencies =
                    {
                        "build"
                    }
                },
                new BauTask
                {
                    Name = "artifacts/output"
                },
                new BauTask
                {
                    Name = "pack",
                    Description = "Create the nuget packages",
                    Dependencies =
                    {
                        "build",
                        "artifacts/output"
                    }
                }
            };
        }

        public class General : TaskListWriterFacts
        {
            [Theory]
            [InlineData(TaskListingKind.TextAll)]
            [InlineData(TaskListingKind.TextDescribed)]
            [InlineData(TaskListingKind.TextPrereq)]
            public void NoTasksProduceNoPlainTextResults(TaskListingKind taskListingKind)
            {
                // arrange
                var tasks = new IBauTask[0];
                var sut = new TaskListWriter(taskListingKind);

                // act
                var actual = sut.CreateTaskListingLines(tasks);

                // assert
                actual.Should().BeEmpty();
            }
        }
        
        public class ListAllTasks : TaskListWriterFacts
        {
            [Fact]
            public void VariousTasksWhereAllAreListedByName()
            {
                // arrange
                var tasks = CreateSampleTasks();
                var maxTaskNameLength = tasks.Where(t => t.Description != null).Max(t => t.Name.Length);

                var expectedNames = tasks.Select(t =>
                {
                    if (t.Description != null)
                    {
                        return t.Name.PadRight(maxTaskNameLength) + " # " + t.Description;
                    }
                    else
                    {
                        return t.Name;
                    }
                }).OrderBy(n => n);

                var sut = this.CreateAllTaskListWriter();

                // act
                var actual = sut.CreateTaskListingLines(tasks);

                // assert
                actual.Should().NotBeNullOrEmpty();
                actual.Select(line => line.ToString())
                    .Should().Equal(expectedNames);
            }

            [Fact]
            public void NamesWithWhitespaceOrHashAreQuoteWrapped()
            {
                // arrange
                var tasks = new[]
                {
                    new BauTask
                    {
                        Name = "some task"
                    },
                    new BauTask
                    {
                        Name = "#hashtag"
                    }
                };

                var sut = this.CreateAllTaskListWriter();

                // act
                var actual = sut.CreateTaskListingLines(tasks);

                // assert
                actual.Should().NotBeNullOrEmpty();
                actual.Select(line => line.ToString()).Should()
                    .Equal(new[]
                    {
                        "\"#hashtag\"",
                        "\"some task\""
                    });
            }

            [Fact]
            public void DescriptionIndentationAppliesToDescribedTasksOnly()
            {
                // arrange
                var tasks = new[]
                {
                    new BauTask
                    {
                        Name = "a-task",
                        Description = "A description."
                    },
                    new BauTask
                    {
                        Name = "Rechtsschutzversicherungsgesellschaften"
                    },
                    new BauTask
                    {
                        Name = "zzzzzzzzzz",
                        Description = "Another description."
                    },
                };

                var sut = this.CreateAllTaskListWriter();

                // act
                var actual = sut.CreateTaskListingLines(tasks);

                // assert
                actual.Should().NotBeNullOrEmpty();
                actual.Select(line => line.ToString()).Should()
                    .Equal(new[]
                    {
                        "a-task     # A description.",
                        "Rechtsschutzversicherungsgesellschaften",
                        "zzzzzzzzzz # Another description."
                    });
            }

            private TaskListWriter CreateAllTaskListWriter()
            {
                return new TaskListWriter(TaskListingKind.TextAll);
            }
        }

        public class ListDescribedTasks : TaskListWriterFacts
        {
            [Fact]
            public void VariousTasksWithDescriptionsAreListed()
            {
                // arrange
                var tasks = CreateSampleTasks();
                var candidateTasks = tasks
                    .Where(t => t.Description != null)
                    .OrderBy(t => t.Name);
                var padAmmount = candidateTasks
                    .Max(x => x.Name.Length)
                    + 1;
                var expectedLines = candidateTasks
                    .Select(t => t.Name.PadRight(padAmmount) + "# " + t.Description);
                var sut = this.CreateDescribedTaskListWriter();

                // act
                var actual = sut.CreateTaskListingLines(tasks);

                // assert
                actual.Should().NotBeNullOrEmpty();
                actual.Select(line => line.ToString())
                    .Should().Equal(expectedLines);
            }

            private TaskListWriter CreateDescribedTaskListWriter()
            {
                return new TaskListWriter(TaskListingKind.TextDescribed);
            }
        }

        public class ListTasksAndPrereqs : TaskListWriterFacts
        {
            [Fact]
            public void VariousTasksWithPrereqsAreListed()
            {
                // arrange
                var tasks = CreateSampleTasks();
                var maxTaskNameLength = tasks.Where(t => t.Description != null).Max(t => t.Name.Length);

                var expectedLines = tasks
                    .OrderBy(t => t.Name)
                    .SelectMany(t =>
                        new[]
                        {
                            t.Description == null ? t.Name : (t.Name.PadRight(maxTaskNameLength) + " # " + t.Description)
                        }
                        .Concat(t.Dependencies
                            .Select(d => "    " + d)))
                    .ToArray();

                var sut = this.CreatePrereqTaskListWriter();

                // act
                var actual = sut.CreateTaskListingLines(tasks).ToArray();

                // assert
                actual.Should().NotBeNullOrEmpty();
                actual.Select(line => line.ToString())
                    .Should().Equal(expectedLines);
            }

            [Fact]
            public void SimpleSampleWithTwoTasks()
            {
                // arrange
                var tasks = new[]
                {
                    new BauTask
                    {
                        Name = "some-task",
                        Dependencies =
                        {
                            "some-other-task"
                        },
                        Description = "Some description."
                    },
                    new BauTask
                    {
                        Name = "some-other-task"
                    }
                };

                var sut = this.CreatePrereqTaskListWriter();

                // act
                var actual = sut.CreateTaskListingLines(tasks);

                // assert
                actual.Should().NotBeNullOrEmpty();
                actual.Select(line => line.ToString()).Should().Equal(new[]
                {
                    "some-other-task",
                    "some-task # Some description.",
                    "    some-other-task"
                });
            }

            private TaskListWriter CreatePrereqTaskListWriter()
            {
                return new TaskListWriter(TaskListingKind.TextPrereq);
            }
        }

        public class ListTasksJson
        {
            [Fact]
            public void EmptyTaskListStillProducesJson()
            {
                var tasks = new BauTask[0];

                var expectedLines = new[]
                {
                    "{",
                    "    \"tasks\": [",
                    "    ]",
                    "}"
                };

                var sut = this.CreateJsonTaskListWriter();

                // act
                var actual = sut.CreateTaskListingLines(tasks);

                // assert
                actual.Should().NotBeNullOrEmpty();
                actual.Select(line => line.ToString())
                    .Should().Equal(expectedLines);
            }

            [Fact]
            public void VariousTasksWithPrereqsAreListed()
            {
                // arrange
                var tasks = CreateSampleTasks();

                var indent1 = "    ";
                var indent2 = indent1 + indent1;
                var indent3 = indent1 + indent2;
                var indent4 = indent1 + indent3;

                Func<string[], string[]> addCommaToAllButTheLast = values =>
                {
                    for (int i = 0; i < values.Length - 1; i++)
                    {
                        values[i] = values[i] + ",";
                    }

                    return values;
                };

                Func<string, string, string> createPropertyLine = (name, value) =>
                    indent3 + "\"" + name + "\": " + (value == null ? "null" : ("\"" + value + "\""));

                Func<string, IEnumerable<string>, IEnumerable<string>> createPropertyArray = (name, values) =>
                    new[]
                    {
                        indent3 + "\"" + name + "\": ["
                    }
                    .Concat(addCommaToAllButTheLast(
                        values
                        .Select(v => indent4 + (v == null ? "null" : "\"" + v + "\""))
                        .ToArray()))
                    .Concat(new[]
                    {
                        indent3 + "]"
                    });

                var expectedLines = new[]
                {
                    "{",
                    indent1 + "\"tasks\": ["
                }
                .Concat(tasks.OrderBy(t => t.Name).SelectMany(t => new[]
                    {
                        indent2 + "{",
                        createPropertyLine("name", t.Name) + ",",
                        createPropertyLine("description", t.Description) + ","
                    }
                    .Concat(createPropertyArray("dependencies", t.Dependencies))
                    .Concat(new[]
                    {
                        indent2 + "},"
                    })))
                .Concat(new[]
                {
                    indent1 + "]",
                    "}"
                })
                .ToArray();

                expectedLines[expectedLines.Length - 3] = indent2 + "}";

                var sut = this.CreateJsonTaskListWriter();

                // act
                var actual = sut.CreateTaskListingLines(tasks);

                // assert
                actual.Should().NotBeNullOrEmpty();
                actual.Select(line => line.ToString())
                    .Should().Equal(expectedLines);
            }

            [Fact]
            public void CanHandleNonAsciiValues()
            {
                var tasks = new[]
                {
                    new BauTask
                    {
                        Name = "😂",
                        Description = "(╯°□°）╯︵ ┻━┻",
                        Dependencies =
                        {
                            "中文维基百科"
                        }
                    },
                    new BauTask
                    {
                        Name = "中文维基百科",
                        Description = "\t\"dquote\" 'squote'\r\n\x01"
                    }
                };

                var expectedLines = new[]
                {
                    "{",
                    "    \"tasks\": [",
                    "        {",
                    "            \"name\": \"😂\",",
                    "            \"description\": \"(╯°□°）╯︵ ┻━┻\",",
                    "            \"dependencies\": [",
                    "                \"中文维基百科\"",
                    "            ]",
                    "        },",
                    "        {",
                    "            \"name\": \"中文维基百科\",",
                    "            \"description\": \"\\t\\\"dquote\\\" 'squote'\\r\\n\\u0001\",",
                    "            \"dependencies\": [",
                    "            ]",
                    "        }",
                    "    ]",
                    "}"
                };

                var sut = this.CreateJsonTaskListWriter();

                // act
                var actual = sut.CreateTaskListingLines(tasks);

                // assert
                actual.Should().NotBeNullOrEmpty();
                actual.Select(line => line.ToString())
                    .Should().Equal(expectedLines);
            }

            private TaskListWriter CreateJsonTaskListWriter()
            {
                return new TaskListWriter(TaskListingKind.Json);
            }
        }
    }
}
