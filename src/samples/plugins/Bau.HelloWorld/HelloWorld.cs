// <copyright file="HelloWorld.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

// Require<Bau>().Task<HelloWorld>().Run();
public class HelloWorld : BauCore.Task
{
    protected override void OnActionsExecuted()
    {
        System.Console.WriteLine("Hello world!");
    }
}
