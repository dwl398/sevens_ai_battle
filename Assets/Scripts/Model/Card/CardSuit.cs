namespace Sevens.Model.Card
{
    /// <summary>
    ///  カードの図柄
    /// </summary>
    public enum CardSuit : int
    {
        None = -1,
        /// <summary>
        /// スペード
        /// </summary>
        Spade,
        /// <summary>
        /// ハート
        /// </summary>
        Heart,
        /// <summary>
        /// ダイヤ
        /// </summary>
        Diamond,
        /// <summary>
        /// クラブ
        /// </summary>
        Club
    }
}