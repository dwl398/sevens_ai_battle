using System.Collections.Generic;

namespace Sevens.View.Card
{
    /// <summary>
    /// 手札を扱うインターフェース
    /// </summary>
    public interface IHand
    {
        /// <summary>
        /// 手札のカード一覧
        /// </summary>
        public IEnumerable<ICard> Cards { get; }
    }
}