// <copyright file="ColoredConsole.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace ColoredConsole
{
    using System;

    public static class StringExtensions
    {
        public static ColorToken Colored(this String text, ConsoleColor? color = ConsoleColor.White)
        {
            return new ColorToken(text, color);
        }
    }
}
