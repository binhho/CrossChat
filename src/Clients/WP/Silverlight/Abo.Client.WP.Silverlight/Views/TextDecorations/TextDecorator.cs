using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace Abo.Client.WP.Silverlight.Views.TextDecorations
{
    public abstract class TextDecorator
    {
        public abstract List<Inline> Decorate(object msg, List<Inline> inlines);

        protected IEnumerable<ResultPair> SplitAndIncludeDelimiters(string text, string[] delimiters)
        {
            if (delimiters.Length == 0)
            {
                yield return new ResultPair { String = text };
                yield break;
            }
            delimiters = delimiters.OrderByDescending(i => i.Length).ToArray();

            string[] split = text.Split(delimiters, StringSplitOptions.None);

            foreach (string part in split)
            {
                if (!string.IsNullOrEmpty(part))
                    yield return new ResultPair { String = part };
                text = SafeSubstring(text, part.Length);

                string delim = delimiters.FirstOrDefault(x => text.StartsWith(x, StringComparison.InvariantCulture));
                if (delim != null)
                {
                    if (!string.IsNullOrEmpty(delim))
                        yield return new ResultPair { String = delim, IsDelimiter = true };
                    text = SafeSubstring(text, delim.Length);
                }
            }
        }

        private string SafeSubstring(string text, int length)
        {
            if (length >= text.Length)
                return string.Empty;
            return text.Substring(length);
        }

        protected class ResultPair
        {
            public string String { get; set; }
            public bool IsDelimiter { get; set; }
        }
    }
}