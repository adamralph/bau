// <copyright file="InlineTasks.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore.Test.Component
{
    using BauCore.Test.Component.Support;
    using FluentAssertions;
    using Xbehave;

    public static class InlineTasks
    {
        [Scenario]
        public static void ExecutingAnInlineTask(ITaskBuilder builder, bool executed)
        {
            "Given a default task with an inline task"
                .f(() => builder = ScriptCs.Require<Bau>().Do(() =>
                {
                    var task = new BauCore.Task();
                    task.Actions.Add(() => executed = true);
                    task.Execute();
                }));

            "When I execute"
                .f(() => builder.Execute());

            "Then the inline task is executed"
                .f(() => executed.Should().BeTrue());
        }
    }
}
