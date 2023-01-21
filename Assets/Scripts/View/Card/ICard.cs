using Sevens.Model.Card;

namespace Sevens.View.Card
{
    /// <summary>
    /// カード情報を扱うインターフェース
    /// </summary>
    public interface ICard
    {
        public CardData CardData { get; }
    }
}