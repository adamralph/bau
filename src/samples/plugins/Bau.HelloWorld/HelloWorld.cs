// <copyright file="HelloWorld.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

// Require<BauPack>().Task<HelloWorld>().Execute();
public class HelloWorld : Bau.Task
{
    protected override void OnActionsExecuted()
    {
        System.Console.WriteLine("Hello world!");
    }
}
