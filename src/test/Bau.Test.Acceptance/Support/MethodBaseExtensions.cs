// <copyright file="MethodBaseExtensions.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Acceptance.Support
{
    using System.Reflection;
    using LiteGuard;

    public static class MethodBaseExtensions
    {
        public static string GetFullName(this MethodBase method)
        {
            Guard.AgainstNullArgument("method", method);

            return string.Concat(method.DeclaringType.FullName, ".", method.Name);
        }
    }
}
