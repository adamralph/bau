namespace Bau.Test.Acceptance.CreateFile
{
    using System;
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(typeof(Program).Assembly.GetName().Name);
            Console.WriteLine("Working directory is: {0}", Directory.GetCurrentDirectory());
            Console.WriteLine("Args: {0}", string.Join(" ", args));

            if (args.Length == 0)
            {
                Console.WriteLine("No filename provided.");
                Environment.Exit(9999);
            }

            Console.WriteLine("Creating file '{0}'...", args[0]);
            File.Create(args[0]);
            Console.WriteLine("Created file '{0}'.", args[0]);
        }
    }
}
