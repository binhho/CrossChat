using System.Collections.Generic;
using Abo.Client.Core.Infrastructure.Contracts;

namespace Abo.Client.WindowsPhone.Infrastructure
{
    /// <summary>
    /// TODO: implement using IsolatedStorageSettings
    /// </summary>
    public class Storage : IStorage
    {
        private readonly Dictionary<string, object> _localStorage = new Dictionary<string, object>();

        public void Set<T>(T obj, string key = "")
        {
            _localStorage[key] = obj;
        }

        public void Set<T>(ref T field, T obj, string key = "")
        {
            field = obj;
            _localStorage[key] = obj;
        }

        public T Get<T>(string key, T defaultValue = default(T))
        {
            object value = defaultValue;
            _localStorage.TryGetValue(key, out value);
            return value is T ? (T)value : defaultValue;
        }
    }
}