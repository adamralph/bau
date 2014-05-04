// <copyright file="Greeter.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

// Require<Bau>().Greeter().Do(g => g.Greeting = "Hello world!").Execute();
namespace BauGreeter
{
    using BauCore;

    public class Greeter : Task
    {
        public string Greeting { get; set; }

        protected override void OnActionsExecuted()
        {
            System.Console.WriteLine(this.Greeting);
        }
    }

    public static class Plugin
    {
        public static IBau<Greeter> Greeter(this IBau bau, string name = Bau.DefaultTask)
        {
            return new Bau<Greeter>(bau, name);
        }
    }
}
