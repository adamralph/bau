// <copyright file="TaskListWriterFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore.Test.Unit
{
    using System.Linq;
    using FluentAssertions;
    using Xunit;

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
            [Fact]
            public void NoTasksProduceNoResults()
            {
                // arrange
                var tasks = new IBauTask[0];
                var sut = new TaskListWriter();

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
                var expectedNames = tasks.Select(t => t.Name).OrderBy(n => n);
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

            private TaskListWriter CreateAllTaskListWriter()
            {
                return new TaskListWriter(); // all defaults should be false
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
                return new TaskListWriter
                {
                    RequireDescription = true,
                    ShowDescription = true
                };
            }
        }

        public class ListTasksAndPrereqs : TaskListWriterFacts
        {
            [Fact]
            public void VariousTasksWithPrereqsAreListed()
            {
                // arrange
                var tasks = CreateSampleTasks();
                var expectedLines = tasks
                    .OrderBy(t => t.Name)
                    .SelectMany(t =>
                        new[]
                        {
                            t.Name
                        }
                        .Concat(t.Dependencies
                            .Select(d => "    " + d)));

                var sut = this.CreatePrereqTaskListWriter();

                // act
                var actual = sut.CreateTaskListingLines(tasks);

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
                        }
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
                    "some-task",
                    "    some-other-task"
                });
            }

            private TaskListWriter CreatePrereqTaskListWriter()
            {
                return new TaskListWriter
                {
                    ShowPrerequisites = true
                };
            }
        }
    }
}
