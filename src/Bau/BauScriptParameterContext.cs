using System;
using System.Collections.Generic;
using System.Dynamic;

namespace BauCore
{
    public class BauScriptParameterContext
    {
        private readonly List<string> keyValuePairs;
        private readonly bool throwIfNull;

        public BauScriptParameterContext(List<string> keyValuePairs, bool throwIfNull = false)
        {
            this.keyValuePairs = keyValuePairs;
            this.throwIfNull = throwIfNull;
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string key]
        {
            get { throw new NotImplementedException(); }
        }
    }
}