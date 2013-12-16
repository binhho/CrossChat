using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Abo.Client.Core.Managers;
using Abo.Client.Core.Model.Astral;
using Abo.Client.Core.Model.Astral.Cards;
using Abo.Server.Application.DataTransferObjects.Messages;

namespace Abo.Client.WP.Silverlight.ViewModels
{
    public class CardsCatalogViewModel : BaseViewModel
    {
        private readonly CardsManager _cardsManager;
        private IEnumerable<ElementInfo> _elements;

        public CardsCatalogViewModel(CardsManager cardsManager)
        {
            _cardsManager = cardsManager;
        }

        public override void Show()
        {
            base.Show();
            Elements = _cardsManager.GetAllCards()
                .GroupBy(i => i.Key)
                .Where(i => i.Key != ElementTypeEnum.Neutral)
                .Select(i => new ElementInfo(i.Key.ToString(), i.SelectMany(j => j.Value).ToArray()))
                .ToArray();
        }

        public IEnumerable<ElementInfo> Elements
        {
            get { return _elements; }
            set { SetProperty(ref _elements, value); }
        }
    }

    [DebuggerDisplay("{Name}, Cards.Count = {Cards.Count}")]
    public class ElementInfo
    {
        public ElementInfo(string name, Card[] cards)
        {
            Name = name;
            Cards = cards;
        }

        public string Name { get; private set; }

        public Card[] Cards { get; private set; }
    }
}
