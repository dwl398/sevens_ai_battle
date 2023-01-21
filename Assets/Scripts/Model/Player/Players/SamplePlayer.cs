using System;
using Sevens.Extension;
using Sevens.View.Card;
using Sevens.View.Field;

namespace Sevens.Model.Player.Players
{
    [Serializable]
    public class SamplePlayer : IPlayer
    {
        public string Name => "でぅるくん";

        public ICard PickUpCard(IField field, IHand hand, int remainPassCount)
        {
            // 出せるかどうかもチェックせずに返す
            return hand.Cards.RandomOne();
        }
    }
}