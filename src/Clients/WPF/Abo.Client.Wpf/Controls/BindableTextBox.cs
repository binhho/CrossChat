using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Abo.Client.Wpf.Controls
{
    public class BindableTextBox : TextBox
    {
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter && EnterCommand != null)
            {
                EnterCommand.Execute(null);
            }
            base.OnKeyDown(e);
        }

        public ICommand EnterCommand
        {
            get { return (ICommand)GetValue(EnterCommandProperty); }
            set { SetValue(EnterCommandProperty, value); }
        }

        public static readonly DependencyProperty EnterCommandProperty =
            DependencyProperty.Register("EnterCommand", typeof(ICommand), typeof(BindableTextBox), new PropertyMetadata(null));
    }
}
