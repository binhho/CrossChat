using System;
using System.IO.IsolatedStorage;
using Abo.Client.Core.Infrastructure.Contracts;

namespace Abo.Client.WP.Silverlight.Infrastructure
{
    public class Storage : IStorage
    {
        public void Set<T>(T obj, string key = "")
        {
            try
            {
                IsolatedStorageSettings.ApplicationSettings[key] = obj;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
            catch (Exception exc)
            {
                //TODO: log
            }
        }

        public void Set<T>(ref T field, T obj, string key = "")
        {
            try
            {
                field = obj;
                Set(obj, key);
            }
            catch (Exception exc)
            {
                //TODO: log
            }
        }

        public T Get<T>(string key, T defaultValue = default(T))
        {
            try
            {
                object value = defaultValue;
                IsolatedStorageSettings.ApplicationSettings.TryGetValue(key, out value);
                return value is T ? (T)value : defaultValue;
            }
            catch (Exception exc)
            {
                return defaultValue;
            }
        }
    }
}
