// <copyright file="ColorConsole.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace ColoredConsole
{
    using System;

    public static class ColorConsole
    {
        private static readonly object @lock = new object();

        public static void WriteLine(ColorText text)
        {
            lock (@lock)
            {
                foreach (var token in text.ToTokenArray())
                {
                    if (token.Color.HasValue)
                    {
                        var originalColor = Console.ForegroundColor;
                        Console.ForegroundColor = token.Color.Value;
                        try
                        {
                            Console.Write(token);
                        }
                        finally
                        {
                            Console.ForegroundColor = originalColor;
                        }
                    }
                    else
                    {
                        Console.Write(token);
                    }
                }

                Console.WriteLine();
            }
        }

        public static void WriteLine(params ColorToken[] tokens)
        {
            if (tokens == null) return;

            WriteLine(new ColorText(tokens));
        }

        public static void WriteLine()
        {
            Console.WriteLine();
        }
    }
}
