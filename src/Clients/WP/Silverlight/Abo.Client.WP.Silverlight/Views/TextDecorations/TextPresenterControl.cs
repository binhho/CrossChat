using System.Windows;
using System.Windows.Controls;

namespace Abo.Client.WP.Silverlight.Views.TextDecorations
{
    public class TextPresenterControl : RichTextBox
    {
        private readonly TextPresenter _textPresenter = new TextPresenter();

        public object Text
        {
            get { return (object)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(object), typeof(TextPresenterControl), new PropertyMetadata(null, (s, e) => ((TextPresenterControl)s).TextChangedStatic()));

        private void TextChangedStatic()
        {
            var p = _textPresenter.Present(Text);
            Blocks.Add(p);
        }
    }
}
