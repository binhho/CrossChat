using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Abo.Client.WP.Silverlight.Views.TextDecorations
{
    public class TextBlockExtensions
    {
        public static Paragraph GetParagraph(DependencyObject obj)
        {
            return (Paragraph)obj.GetValue(ParagraphProperty);
        }

        public static void SetParagraph(DependencyObject obj, Paragraph value)
        {
            obj.SetValue(ParagraphProperty, value);
        }

        public static readonly DependencyProperty ParagraphProperty =
            DependencyProperty.RegisterAttached("Paragraph", typeof(Paragraph), typeof(TextBlockExtensions), new PropertyMetadata(null, OnParagraphChanged));

        private static void OnParagraphChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = d as RichTextBox;
            if (textBlock == null)
                throw new InvalidOperationException("Only RichTextBlock is allowed to be host.");

            Paragraph paragraph = GetParagraph(textBlock);
            if (paragraph == null)
            {
                textBlock.Blocks.Clear();
            }
            else
            {
                textBlock.Blocks.Clear();
                textBlock.Blocks.Add(paragraph);
            }
        }
    }
}