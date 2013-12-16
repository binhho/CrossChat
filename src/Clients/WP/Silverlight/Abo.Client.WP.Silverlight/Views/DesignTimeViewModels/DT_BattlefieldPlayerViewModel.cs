using System.Collections.ObjectModel;
using Abo.Client.Core.Model.Astral;
using Abo.Client.Core.Model.Astral.Cards;
using Abo.Server.Application.DataTransferObjects.Messages;

namespace Abo.Client.WP.Silverlight.Views.DesignTimeViewModels
{
    public class DT_BattlefieldPlayerViewModel
    {
        private Element[] _elements = null;

        public Element[] Elements
        {
            get
            {
                return _elements ?? (_elements = new[]
                {
                    new Element {ElementType = ElementTypeEnum.Fire, Mana = 8, Cards = CreateCards(1)},
                    new Element {ElementType = ElementTypeEnum.Water, Mana = 8, Cards = CreateCards(2)},
                    new Element {ElementType = ElementTypeEnum.Air, Mana = 8, Cards = CreateCards(3)},
                    new Element {ElementType = ElementTypeEnum.Earth, Mana = 8, Cards = CreateCards(4)},
                    new Element {ElementType = ElementTypeEnum.Demonic, Mana = 8, Cards = CreateCards(5)},
                });
            }
        }

        private ObservableCollection<Card> CreateCards(int i)
        {
            return new ObservableCollection<Card>
            {
                new CreatureCard {Name = "FireElemental", ElementType = ElementTypeEnum.Fire, Cost = i, Health = 39, Damage = 0},
                new CreatureCard {Name = "WaterElemental", ElementType = ElementTypeEnum.Water, Cost = i, Health = 5, Damage = 11},
                new SpellCard    {Name = "Inferno", ElementType = ElementTypeEnum.Fire, Cost = i },
                new CreatureCard {Name = "EarthElemental", ElementType = ElementTypeEnum.Earth, Cost = i, Health = 39, Damage = 0},
                new CreatureCard {Name = "Banshee", ElementType = ElementTypeEnum.Death, Cost = i, Health = 5, Damage = 11},
                new CreatureCard {Name = "AncientDragon", ElementType = ElementTypeEnum.Beast, Cost = i, Health = 25, Damage = 8},
            };
        }

        public Element SelectedElement
        {
            get { return Elements[2]; }
            set { }
        }
    }
}