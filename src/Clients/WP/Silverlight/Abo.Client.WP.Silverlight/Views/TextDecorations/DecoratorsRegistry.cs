using System.Collections.Generic;
using Abo.Client.WP.Silverlight.Views.TextDecorations.Decorators;

namespace Abo.Client.WP.Silverlight.Views.TextDecorations
{
    public static class DecoratorsRegistry
    {
        private static readonly List<TextDecorator> _decorators = new List<TextDecorator>(8);

        static DecoratorsRegistry()
        {
            _decorators.Clear();
            //the order is important
            Register(new TimestampDecorator());
            Register(new NickDecorator());
            Register(new DefaultDecorator());
            Register(new HyperlinkDecorator());
        }

        /// <summary>
        /// Register new decorator
        /// </summary>
        public static void Register(TextDecorator decorator)
        {
            _decorators.Add(decorator);
        }

        public static IEnumerable<TextDecorator> Decorators { get { return _decorators; } }
    }
}