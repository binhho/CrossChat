using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Abo.Client.WP.Silverlight.Resources;
using Abo.Client.WP.Silverlight.ViewModels.Messages;

namespace Abo.Client.WP.Silverlight.Views.TextDecorations.Decorators
{
    public class DefaultDecorator : TextDecorator
    {
        #region Implementation of IMessageDecorator

        /// <summary>
        /// Transform collection of inlines
        /// </summary>
        public override List<Inline> Decorate(object msg, List<Inline> inlines)
        {
            var item = msg as MessageViewModel;
            if (item != null)
            {
                var ban = item as BanNotificationViewModel;
                var devoice = item as DevoiceNotificationViewModel;
                var grantedModership = item as GrantedModershipNotificationViewModel;
                var removedModership = item as RemovedModershipNotificationViewModel;
                var textVm = item as TextMessageViewModel;

                var systemBrush = TextDecorationStyles.ChatSystemForeground;

                if (ban != null)
                {
                    inlines.Add(Decorate(ban.ActorName, true, systemBrush));
                    inlines.Add(Decorate(" " + AppResources.XbannedY + " ", false, systemBrush));
                    inlines.Add(Decorate(ban.TargetName, true, systemBrush));
                    inlines.Add(Decorate(" (" + ban.Reason + ")", false, systemBrush));
                }
                else if (devoice != null)
                {
                    inlines.Add(Decorate(devoice.ActorName, true, systemBrush));
                    inlines.Add(Decorate(" " + AppResources.XdevoicedY + " ", false, systemBrush));
                    inlines.Add(Decorate(devoice.TargetName, true, systemBrush));
                    inlines.Add(Decorate(" (" + devoice.Reason + ")", false, systemBrush));
                }
                else if (grantedModership != null)
                {
                    inlines.Add(Decorate(grantedModership.ActorName, true, systemBrush));
                    inlines.Add(Decorate(" " + AppResources.IsNowModer, false, systemBrush));
                }
                else if (removedModership != null)
                {
                    inlines.Add(Decorate(removedModership.ActorName, true, systemBrush));
                    inlines.Add(Decorate(" " + AppResources.IsNotModerAnymore, false, systemBrush));
                }
                else if (textVm != null)
                {
                    inlines.Add(Decorate(textVm.Body));
                }
                else
                {
                    inlines.Add(Decorate(item.ToString()));
                }
            }
            else
            {
                inlines.Add(Decorate(msg));
            }
            return inlines;
        }

        public static Inline Decorate(object message)
        {
            var brush = TextDecorationStyles.ChatNormalForeground;
            var messageInline = new Run
            {
                Text = message.ToString(),
                FontWeight = FontWeights.Normal,
                Foreground = brush
            };
            return messageInline;
        }

        private static Inline Decorate(string text, bool bold, Brush brush)
        {
            var messageInline = new Run
            {
                Text = text,
                FontWeight = bold ? FontWeights.Bold : FontWeights.Normal,
                Foreground = brush
            };
            return messageInline;
        }

        #endregion
    }
}