// <copyright file="ColorToken.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;

    public struct ColorToken
    {
        private readonly string text;
        private readonly ConsoleColor? color;

        public ColorToken(string text)
            : this(text, null)
        {
        }

        public ColorToken(string text, ConsoleColor? color)
        {
            this.text = text;
            this.color = color;
        }

        public string Text
        {
            get { return this.text; }
        }

        public ConsoleColor? Color
        {
            get { return this.color; }
        }

        public static implicit operator ColorToken(string text)
        {
            return new ColorToken(text);
        }

        public ColorToken Coalesce(ConsoleColor color)
        {
            return new ColorToken(this.text, this.color ?? color);
        }

        public override string ToString()
        {
            return this.text;
        }
    }
}
