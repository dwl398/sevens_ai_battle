using Sevens.View.Card;
using Sevens.View.Field;
using Sevens.Extension;

namespace Sevens.Model.Player.Players
{
    public class SolidPlayer : IPlayer
    {
        public string Name => "まじめでぅる";

        public ICard PickUpCard(IField field, IHand hand, int remainPassCount)
        {
            // 置けるやつをとにかく返す
            foreach (var card in hand.Cards)
            {
                if (field.CanAdd(card))
                {
                    return card;
                }
            }

            return hand.Cards.RandomOne();
        }
    }
}