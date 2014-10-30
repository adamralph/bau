// <copyright file="ColorToken.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace ColoredConsole
{
    using System;

    public struct ColorToken : IEquatable<ColorToken>
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

        public static bool operator ==(ColorToken left, ColorToken right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ColorToken left, ColorToken right)
        {
            return !left.Equals(right);
        }

        public ColorToken Coalesce(ConsoleColor defaultColor)
        {
            return new ColorToken(this.text, this.color ?? defaultColor);
        }

        public override string ToString()
        {
            return this.text;
        }

        public override int GetHashCode()
        {
            return this.text == null ? 0 : this.text.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is ColorToken && this.Equals((ColorToken)obj);
        }

        public bool Equals(ColorToken other)
        {
            return
                this.text == other.text &&
                this.color == other.color;
        }
    }
}
