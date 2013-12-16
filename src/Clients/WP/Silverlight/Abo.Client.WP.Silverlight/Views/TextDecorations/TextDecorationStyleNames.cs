using System.Windows;
using System.Windows.Media;

namespace Abo.Client.WP.Silverlight.Views.TextDecorations
{
    public static class TextDecorationStyles
    {
        public static Brush ChatNormalForeground { get { return GetBrush("ChatNormalForeground"); }}
        public static Brush ChatSystemForeground { get { return GetBrush("ChatSystemForeground"); }}
        public static Brush ChatHyperlinkForeground { get { return GetBrush("ChatHyperlinkForeground"); }}
        public static Brush ChatPlayerNickForeground { get { return GetBrush("ChatPlayerNickForeground"); }}
        public static Brush ChatModerNickForeground { get { return GetBrush("ChatModerNickForeground"); }}
        public static Brush ChatAdminNickForeground { get { return GetBrush("ChatAdminNickForeground"); } }
        public static Brush ChatTimestampForeground{ get { return GetBrush("ChatTimestampForeground"); }}

        private static Brush GetBrush(string key)
        {
            return Application.Current.Resources[key] as Brush;
        }
    }
}
