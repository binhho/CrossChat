using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Abo.Utils;

namespace Abo.Client.WP.Silverlight.Seedwork.Controls
{
    public class AutoscrolledListBox : ListBox
    {
        public object ObservableItems
        {
            get { return (object)GetValue(ObservableItemsProperty); }
            set { SetValue(ObservableItemsProperty, value); }
        }

        public static readonly DependencyProperty ObservableItemsProperty =
            DependencyProperty.Register("ObservableItems", typeof(object), typeof(AutoscrolledListBox), new PropertyMetadata(null, (s, e) => ((AutoscrolledListBox)s).ObservableItemsChanged(e)));

        private void ObservableItemsChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is INotifyCollectionChanged)
            {
                ((INotifyCollectionChanged)e.OldValue).CollectionChanged -= AutoscrolledListBox_OnCollectionChanged;
                ItemsSource = null;
            }
            if (e.NewValue is INotifyCollectionChanged)
            {
                var source = ((INotifyCollectionChanged)e.NewValue);
                source.CollectionChanged += AutoscrolledListBox_OnCollectionChanged;
                ItemsSource = source as IEnumerable;
            }
        }

        private void AutoscrolledListBox_OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SubscribeToEvents();
        }

        public async void ScrollToBottom()
        {
            if (_scroller != null)
            {
                await Task.Delay(300);

                if (Items.IsNullOrEmpty())
                    return;
                var selectedIndex = Items.Count - 1;
                if (selectedIndex < 0)
                    return;

                SelectedIndex = selectedIndex;
                UpdateLayout();
                ScrollIntoView(SelectedItem);
            }
        }

        private bool _isSubscribed = false;
        private ScrollViewer _scroller = null;

        private void SubscribeToEvents()
        {
            _scroller = FindChildOfType<ScrollViewer>(this);
            if (_isSubscribed || _scroller == null)
                return;
            _isSubscribed = true;

            Binding binding = new Binding();
            binding.Source = _scroller;
            binding.Path = new PropertyPath("ExtentHeight");
            SetBinding(ScrollViewerHeightProperty, binding);

            _scroller.SizeChanged += scroller_OnSizeChanged;
            ScrollToBottom();
        }

        internal static T FindChildOfType<T>(DependencyObject root) where T : class
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                DependencyObject current = queue.Dequeue();
                for (int i = VisualTreeHelper.GetChildrenCount(current) - 1; 0 <= i; i--)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    var typedChild = child as T;
                    if (typedChild != null)
                    {
                        return typedChild;
                    }
                    queue.Enqueue(child);
                }
            }
            return null;
        }

        public double ScrollViewerHeight
        {
            get { return (double)GetValue(ScrollViewerHeightProperty); }
            set { SetValue(ScrollViewerHeightProperty, value); }
        }

        public static readonly DependencyProperty ScrollViewerHeightProperty =
            DependencyProperty.Register("ScrollViewerHeight", typeof(double), typeof(AutoscrolledListBox), new PropertyMetadata(0.0d, (s, e) => ((AutoscrolledListBox)s).ScrollViewerHeightChanged()));

        private void ScrollViewerHeightChanged()
        {
            ScrollToBottom();
        }

        private void scroller_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ScrollToBottom();
        }
    }
}
