using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Abo.Client.WP.Silverlight.Seedwork.Controls
{
    public class TapGrid : Grid
    {
        public TapGrid()
        {
            this.Tap += TiltGridTap;
        }

        void TiltGridTap(object sender, GestureEventArgs e)
        {
            if (TapCommand != null)
                TapCommand.Execute(TapCommandParameter);
        }

        public ICommand TapCommand
        {
            get { return (ICommand)GetValue(TapCommandProperty); }
            set { SetValue(TapCommandProperty, value); }
        }

        public static readonly DependencyProperty TapCommandProperty =
            DependencyProperty.Register("TapCommand", typeof(ICommand), typeof(TiltGrid), new PropertyMetadata(null));

        public object TapCommandParameter
        {
            get { return (object)GetValue(TapCommandParameterProperty); }
            set { SetValue(TapCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty TapCommandParameterProperty =
            DependencyProperty.Register("TapCommandParameter", typeof(object), typeof(TapGrid), new PropertyMetadata(null));
    }
}