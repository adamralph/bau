// <copyright file="RenablingTasks.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore.Test.Component
{
    using System.Collections.Generic;
    using BauCore.Test.Component.Support;
    using FluentAssertions;
    using Xbehave;

    public static class RenablingTasks
    {
        [Scenario]
        public static void ReenablingATask(ITaskBuilder builder, List<string> executedTasks)
        {
            "Given a non-default task which depends on and reenables another non-default task"
                .f(c => builder = ScriptCs.Require<Bau>()
                    .Task("non-default").DependsOn("other-non-default")
                        .Do(() =>
                        {
                            builder.Reenable("other-non-default");
                            executedTasks.Add("non-default");
                        }));

            "And the other non-default task"
                .f(c => builder
                    .Task("other-non-default")
                        .Do(() => (executedTasks ?? (executedTasks = new List<string>())).Add("other-non-default")));

            "And a default task which depends on both non-default tasks"
                .f(() => builder
                    .Task("default").DependsOn("non-default", "other-non-default")
                        .Do(() => executedTasks.Add("default")));

            "When I execute"
                .f(() => builder.Execute());

            "Then four tasks are executed"
                .f(() => executedTasks.Count.Should().Be(4));

            "And the other non-default task is executed first"
                .f(() => executedTasks[0].Should().Be("other-non-default"));
            
            "And the non-default task is executed second"
                .f(() => executedTasks[1].Should().Be("non-default"));

            "And the other non-default task is executed third for the second time"
                .f(() => executedTasks[2].Should().Be("other-non-default"));

            "And the default task is executed fourth"
                .f(() => executedTasks[3].Should().Be("default"));
        }
    }
}
