// <copyright file="ColorText.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;
    using System.Linq;

    public struct ColorText
    {
        private readonly ColorToken[] tokens;

        public ColorText(params ColorToken[] tokens)
        {
            this.tokens = tokens.ToArray();
        }

        public static implicit operator ColorText(string text)
        {
            return new ColorText(text);
        }

        public static implicit operator ColorText(ColorToken text)
        {
            return new ColorText(text);
        }

        public ColorToken[] ToTokenArray()
        {
            return this.tokens.ToArray();
        }

        public ColorText Concat(ColorText text)
        {
            return new ColorText(this.tokens.Concat(text.tokens).ToArray());
        }

        public ColorText Coalesce(ConsoleColor color)
        {
            return new ColorText(this.tokens.Select(token => token.Coalesce(color)).ToArray());
        }

        public override string ToString()
        {
            return string.Concat(this.tokens.Select(token => token.ToString()));
        }
    }
}
