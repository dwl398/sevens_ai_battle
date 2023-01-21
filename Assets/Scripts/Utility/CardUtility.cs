using Sevens.View.Card;
using UnityEngine;

namespace Sevens.Utility
{
    public static class CardUtility
    {
        /// <summary>
        /// フィールド配置時のカードのサイズ
        /// </summary>
        public static readonly Vector2 CardFieldSize = new Vector2(200, 300);

        /// <summary>
        /// ハンド配置時のカードのサイズ
        /// </summary>
        public static readonly Vector2 CarHandSize = new Vector2(150, 225);

        /// <summary>
        /// カードの座標を取得
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static Vector2 GetPositionByCard(ICard card)
        {
            const int startPositionX = 105;
            const int startPositionY = -170;
            const int offsetX = 205;
            const int offsetY = -305;
            return new Vector2(startPositionX + offsetX * (int)(card.CardData.Number - 1), startPositionY + offsetY * (int)card.CardData.Suit);
        }
    }
}