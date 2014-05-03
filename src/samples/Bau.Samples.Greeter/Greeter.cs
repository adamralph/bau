// <copyright file="Greeter.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

// Require<BauPack>().Greeter().Do(g => g.Greeting = "Hello world!").Execute();
namespace BauGreeter
{
    using Bau;

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
        public static IBauPack<Greeter> Greeter(this IBauPack bau, string name = BauPack.DefaultTask)
        {
            return new BauPack<Greeter>(bau, name);
        }
    }
}
