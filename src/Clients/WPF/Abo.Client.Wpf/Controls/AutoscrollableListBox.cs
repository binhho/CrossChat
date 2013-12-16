using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Abo.Client.Wpf.Controls
{
    public class AutoscrollableListBox : ListBox
    {
        public new IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public new static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(AutoscrollableListBox), new PropertyMetadata(null, (s, e) => ((AutoscrollableListBox)s).ItemsSourceChanged()));

        private void ItemsSourceChanged()
        {
            var notifyCollectionChanged = ItemsSource as INotifyCollectionChanged;
            if (notifyCollectionChanged != null)
            {
                base.ItemsSource = this.ItemsSource;
                notifyCollectionChanged.CollectionChanged += notifyCollectionChanged_OnCollectionChanged;
            }
        }

        private void notifyCollectionChanged_OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                ScrollIntoView(e.NewItems[0]);
                SelectedItem = e.NewItems[0];
            }
        }
    }
}
