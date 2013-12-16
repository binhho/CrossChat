using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;

namespace Abo.Client.WP.Silverlight.Views.TextDecorations
{
    public class TextPresenter : IValueConverter
    {
        public Paragraph Present(object message)
        {
            var paragraph = new Paragraph();
            var textInlines = new List<Inline>();

            foreach (var decorator in DecoratorsRegistry.Decorators)
            {
                textInlines = decorator.Decorate(message, textInlines);
            }

            foreach (var textInline in textInlines)
            {
                paragraph.Inlines.Add(textInline);
            }

            return paragraph;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            if (value != null)
            {
                return Present(value);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotSupportedException();
        }
    }
}