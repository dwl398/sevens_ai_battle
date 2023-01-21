using System.Collections.Generic;
using System.Linq;
using Sevens.Extension;
using Sevens.Model.Card;
using Sevens.Utility;
using Sevens.View.Card;
using UnityEngine;

namespace Sevens.View.Field
{
    /// <summary>
    /// カードを置く場所
    /// </summary>
    public class Field : MonoBehaviour , IField
    {
        [SerializeField] private List<Card.Card> cards;

        public IEnumerable<ICard> Cards => cards.Select(x => x as ICard);

        /// <summary>
        /// 7以外のカードを1枚選んでフィールドから取り除く
        /// </summary>
        /// <returns></returns>
        public Card.Card PopCardByRandomWithoutSeven()
        {
            var card = cards.Where(x => x.CardData.Number != CardNumber.Seven).RandomOne();
            return PopCard(card);
        }

        /// <summary>
        /// カードをフィールドから取り除く
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public Card.Card PopCard(Card.Card card)
        {
            cards.Remove(card);
            return card;
        }

        /// <summary>
        /// カードをフィールドに置く
        /// </summary>
        /// <param name="card"></param>
        public void PushCard(Card.Card card)
        {
            if (cards.Contains(card))
            {
                return;
            }

            cards.Add(card);
            card.transform.SetParent(transform, false);
            card.RectTransform.anchoredPosition = CardUtility.GetPositionByCard(card);
            card.RectTransform.sizeDelta = CardUtility.CardFieldSize;
        }

        /// <summary>
        /// 追加できるかチェック
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool CanAdd(ICard card)
        {
            // 図柄でフィールドをフィルタして数値でソート
            var list = cards.Where(x => x.CardData.Suit == card.CardData.Suit).OrderBy(x => x.CardData.Number);

            // 穴あきがないかどうかをチェックする
            var temp = list.ToList();
            var buried = true;
            var removeList = new List<Card.Card>();
            // 7からAまでチェック
            for (var i = temp.Count - 1; 0 <= i ; i--)
            {
                // 7より大きいのはチェックしない
                if (CardNumber.Seven <= temp[i].CardData.Number)
                {
                    continue;
                }

                // 最後までチェックしたら終了
                if (i - 1 < 0)
                {
                    break;
                }

                // 6 - 1 = 5 だよね？ってコト
                if (temp[i].CardData.Number - 1 != temp[i - 1].CardData.Number)
                {
                    // 以降の部分を除外リストにいれてチェック再開する
                    buried = false;
                    for (var k = i - 1; 0 <= k; k--)
                    {
                        removeList.Add(temp[k]);
                    }
                    break;
                }
            }

            // 7からKまでチェック
            for (var i = 0; i < temp.Count; i++)
            {
                // 7より小さいのはチェックしない
                if (temp[i].CardData.Number <= CardNumber.Seven)
                {
                    continue;
                }

                // 最後までチェックしたら終了
                if (temp.Count <= i + 1)
                {
                    break;
                }

                // 10 + 1 = 11 だよね？ってコト
                if (temp[i].CardData.Number + 1 != temp[i + 1].CardData.Number)
                {
                    // 以降の部分を除外リストにいれてチェック再開する
                    buried = false;
                    for (var k = i + 1; k < temp.Count; k++)
                    {
                        removeList.Add(temp[k]);
                    }
                    break;
                }
            }

            // 穴あき状態なら除外リストのデータをリストから除外する
            if (buried == false)
            {
                foreach (var remove in removeList)
                {
                    temp.Remove(remove);
                }

                // 削除した後のリストをソートして渡す
                list = temp.OrderBy(x => x.CardData.Number);
            }

            var first = list.First();
            var last = list.Last();

            // 同じ図柄の最小の1つ前と最大の1つ後の数字なら置ける
            if (card.CardData.Number == (first.CardData.Number - 1) || card.CardData.Number == (last.CardData.Number + 1))
            {
                return true;
            }

            return false;
        }
    }
}