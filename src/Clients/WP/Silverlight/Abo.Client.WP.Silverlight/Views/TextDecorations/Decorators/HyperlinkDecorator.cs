using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Documents;

namespace Abo.Client.WP.Silverlight.Views.TextDecorations.Decorators
{
    public class HyperlinkDecorator : TextDecorator
    {
        #region Implementation of IMessageDecorator

        public override List<Inline> Decorate(object message, List<Inline> inlines)
        {
            const string pattern = "(http|https|ftp|mailto|xmpp)://([a-zA-Zа-яА-Я0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?";

            for (int i = 0; i < inlines.Count; i++)
            {
                var inline = inlines[i] as Run;
                if (inline == null)
                    continue;

                string[] matches = Regex.Matches(inline.Text, pattern, RegexOptions.IgnoreCase).OfType<Match>().Select(item => item.Value).ToArray();
                if (matches.Length < 1)
                    continue;

                inlines.RemoveAt(i);

                string[] parts = SplitAndIncludeDelimiters(inline.Text, matches).Select(p => p.String).ToArray();
                for (int j = i; j < parts.Length + i; j++)
                {
                    string part = parts[j - i];
                    if (matches.Contains(part) && IsWellFormedUriString(part))
                        inlines.Insert(j, DecorateAsHyperlink(part));
                    else
                        inlines.Insert(j, DefaultDecorator.Decorate(part));
                }
            }
            return inlines;
        }

        private static bool IsWellFormedUriString(string uri)
        {
            try
            {
                return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
            }
            catch
            {
                return false;
            }
        }

        private Inline DecorateAsHyperlink(string text)
        {
            var hyperlink = new Hyperlink
            {
                NavigateUri = new Uri(text, UriKind.Absolute),
            };

            var linkRun = DefaultDecorator.Decorate(text);
            linkRun.Foreground = TextDecorationStyles.ChatHyperlinkForeground;
            hyperlink.Inlines.Add(linkRun);
            hyperlink.TargetName = "_blank";

            //hyperlink.Click += HyperlinkClickHandler;
            return hyperlink;
        }


        #endregion
    }
}