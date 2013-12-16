using System.Windows;
using System.Windows.Input;
using Microsoft.Phone.Controls;

namespace Abo.Client.WP.Silverlight.Seedwork.Controls
{
    public class PivotEx : Pivot
    {
        protected override void OnSelectionChanged(System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selection = SelectedItem as PivotItem;
            if (selection != null)
            {
                var command = GetSelectionCommand(selection);
                if (command != null)
                    command.Execute(null);
            }
            base.OnSelectionChanged(e);
        }

        public static ICommand GetSelectionCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(SelectionCommandProperty);
        }

        public static void SetSelectionCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(SelectionCommandProperty, value);
        }

        public static readonly DependencyProperty SelectionCommandProperty =
            DependencyProperty.RegisterAttached("SelectionCommand", typeof(ICommand), typeof(PivotEx), new PropertyMetadata(null));
    }

}
