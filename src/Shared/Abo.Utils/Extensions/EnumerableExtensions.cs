using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Abo.Utils //common namespace to see these methods everywhere without usings
{
    public static class EnumerableExtensions
    {        
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source) 
            {
                action(item);
            }
        }

        public static T GetAndRemove<T>(this IDictionary<object, object> source, object key)
        {
            object value;
            if (source.TryGetValue(key, out value))
            {
                source.Remove(key);
                return (T) value;
            }
            else
            {
                throw new KeyNotFoundException(String.Format("object with key {0} doesn't exist", key));       
            }
        }

        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        {
            var result = new List<IEnumerable<T>>();
            var chunk = new List<T>();
            foreach (var item in source)
            {
                if (chunk.Count >= chunksize)
                {
                    result.Add(chunk.ToArray());
                    chunk.Clear();
                }
                chunk.Add(item);
            }
            result.Add(chunk);
            return result;
        }
        
        public static bool Any(this IEnumerable source) 
        {
            foreach (var item in source) 
            {
                return true; //just one call of MoveNext is needed
            }
            return false;
        }

        public static bool IsNullOrEmpty(this IEnumerable enumerable) 
        {
            return enumerable == null || !Any(enumerable);
        }

        public static void RemoveAll<T>(this Collection<T> collection, Func<T, bool> selector)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                var item = collection[i];
                if (selector(item))
                {
                    collection.Remove(item);
                    i--;
                }
            }
        }

        public static void InsertFirstIfUnique(this IList list, object obj)
        {
            if (list.Count == 0)
            {
                list.Insert(0, obj);
            }
            else if (!list[0].Equals(obj))
            {
                list.Insert(0, obj);
            }
        }

        public static void InvokeIfNotNull(this Action action)
        {
            if (action != null)
            {
                action.Invoke();
            }
        }

        public static Dictionary<TKey, TElement> ToDictionarySafe<TSource, TKey, TElement>(this IEnumerable<TSource> source, 
            Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            var dictionary = new Dictionary<TKey, TElement>();
            foreach (TSource item in source)
                dictionary[keySelector(item)] = elementSelector(item);
            return dictionary;
        }

        public static string CutIfLonger(this string source, int maxLength = 1000)
        {
            if (String.IsNullOrEmpty(source))
                return source;
            if (source.Length < maxLength)
                return source;
            return source.Remove(maxLength - 1);
        }

        public static DateTime ToTime(this DateTime dateTime)
        {
            return new DateTime(1900, 1, 1, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }
    }
}

