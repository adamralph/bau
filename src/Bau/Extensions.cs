// <copyright file="Extensions.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    public static class Extensions
    {
        public static string Truncate(this string text, int maxLength)
        {
            if (text == null || text.Length <= maxLength)
            {
                return text;
            }

            return string.Concat(text.Substring(0, maxLength - 3), "...");
        }
    }
}
