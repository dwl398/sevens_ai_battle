using Sevens.View.Card;
using Sevens.View.Field;

namespace Sevens.Model.Player
{
    public interface IPlayer
    {
        /// <summary>
        /// 名前
        /// </summary>
        string Name { get; }

        /// <summary>
        /// カード選択
        /// </summary>
        /// <param name="field">カード置き場の状態</param>
        /// <param name="hand">手札</param>
        /// <param name="remainPassCount">残りパス回数</param>
        /// <returns>返すカード,出せないカードかnullならパス扱いになる</returns>
        ICard PickUpCard(IField field, IHand hand, int remainPassCount);
    }
}