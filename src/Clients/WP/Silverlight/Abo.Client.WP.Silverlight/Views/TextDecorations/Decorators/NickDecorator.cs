using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Abo.Client.WP.Silverlight.Resources;
using Abo.Client.WP.Silverlight.ViewModels.Messages;

namespace Abo.Client.WP.Silverlight.Views.TextDecorations.Decorators
{
    public class NickDecorator : TextDecorator
    {
        public override List<Inline> Decorate(object msg, List<Inline> inlines)
        {
            var item = msg as MessageViewModel;
            if (item != null)
            {
                var textVm = item as TextMessageViewModel;
                Brush nickBrush = null;

                string author;
                if (textVm != null)
                {
                    author = textVm.AuthorName;
                    if (textVm.IsAdmin)
                    {
                        nickBrush = TextDecorationStyles.ChatAdminNickForeground;
                    }
                    else if (textVm.IsModerator)
                    {
                        nickBrush = TextDecorationStyles.ChatModerNickForeground;
                    }
                    else
                    {
                        nickBrush = TextDecorationStyles.ChatPlayerNickForeground;
                    }
                }
                else
                {
                    author = AppResources.SystemNick;
                    nickBrush = TextDecorationStyles.ChatSystemForeground;
                }

                Run nick = new Run { Text = author + ": ", Foreground = nickBrush, FontWeight = FontWeights.Normal };
                inlines.Add(nick);
            }

            return inlines;
        }

        public static Color ToColor(long argb)
        {
            return Color.FromArgb((byte)((argb & -16777216) >> 0x18),
                (byte)((argb & 0xff0000) >> 0x10),
                (byte)((argb & 0xff00) >> 8),
                (byte)(argb & 0xff));
        }
    }
}