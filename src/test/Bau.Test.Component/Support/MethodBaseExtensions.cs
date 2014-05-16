// <copyright file="MethodBaseExtensions.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore.Test.Component.Support
{
    using System.Reflection;
    using LiteGuard;

    public static class MethodBaseExtensions
    {
        public static string GetFullName(this MethodBase method)
        {
            Guard.AgainstNullArgument("method", method);

            return method.DeclaringType == null
                ? method.Name
                : string.Concat(method.DeclaringType.FullName, ".", method.Name);
        }
    }
}
