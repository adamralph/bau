// <copyright file="Plugin.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

// Baufile.csx
// Require<BauPack>().Greeter().Do(g => g.Greeting = "Hello world!").Execute();
namespace Bau.Samples.Greeter
{
    using ScriptCs.Contracts;

    public static class Plugin
    {
        public static IBauPack<GreeterTask> Greeter(this IBauPack bau, string name = BauPack.DefaultTask)
        {
            return new BauPack<GreeterTask>(bau, name);
        }
    }

    public class GreeterTask : Task
    {
        public string Greeting { get; set; }

        public override void Execute()
        {
            base.Execute();
            System.Console.WriteLine(this.Greeting);
        }
    }

    public class Pack : ScriptPack<BauGreeter>
    {
        public override void Initialize(IScriptPackSession session)
        {
            session.ImportNamespace(this.GetType().Namespace);
            this.Context = new BauGreeter();
        }
    }

    public class BauGreeter : IScriptPackContext
    {
    }
}
