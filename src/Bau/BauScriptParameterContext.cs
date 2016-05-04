using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace BauCore
{
    public class BauScriptParameterContext : DynamicObject
    {
        private readonly bool throwIfNull;
        private readonly Dictionary<string, string> parameterDictionary; 

        public BauScriptParameterContext(IEnumerable<string> keyValuePairs, bool throwIfNull = false)
        {
            this.throwIfNull = throwIfNull;
            this.parameterDictionary = keyValuePairs.ToDictionary(k => k.Split("=".ToCharArray()).First(),
                                                                  v => v.Split("=".ToCharArray()).Last());
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var memberName = binder.Name;
            return GetValue(memberName, out result);
        }

        private bool GetValue(string key, out object result)
        {
            result = null;
            var status = false;
            if (parameterDictionary.ContainsKey(key))
            {
                result = parameterDictionary[key];
                status = true;
            }
            if (!status && throwIfNull)
            {
                throw new KeyNotFoundException(string.Format("Unable to find a matching key for {0}", key));
            }
            return status;
        }

        public string this[string key]
        {
            get { return parameterDictionary[key]; }
        }

        public int Count
        {
            get { return parameterDictionary.Count; }
        }
    }
}
