using System.Collections.Generic;
using Sevens.View.Card;

namespace Sevens.View.Field
{
    /// <summary>
    /// カード置き場
    /// </summary>
    public interface IField
    {
        /// <summary>
        /// カード置き場にあるカード一覧
        /// </summary>
        public IEnumerable<ICard> Cards { get; }

        /// <summary>
        /// 置けるかどうか
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool CanAdd(ICard card);
    }
}