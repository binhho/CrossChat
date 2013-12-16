using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Media;
using Abo.Client.WP.Silverlight.ViewModels.Messages;

namespace Abo.Client.WP.Silverlight.Views.TextDecorations.Decorators
{
    public class TimestampDecorator : TextDecorator
    {
        public override List<Inline> Decorate(object msg, List<Inline> inlines)
        {
            var item = msg as MessageViewModel;
            if (item != null)
            {
                Run date = new Run
                {
                    Text = item.Timestamp.ToString("[HH:mm] "),
                    Foreground = TextDecorationStyles.ChatTimestampForeground,
                    FontFamily = new FontFamily("Verdana"),
                    FontSize = 14
                };
                inlines.Insert(0, date);
            }
            return inlines;
        }
    }
}
