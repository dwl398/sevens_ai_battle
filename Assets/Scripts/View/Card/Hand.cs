using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sevens.Model;
using Sevens.Model.Card;
using Sevens.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sevens.View.Card
{
    public class Hand : MonoBehaviour , IHand
    {
        [SerializeField] private TextMeshProUGUI nameLabel;

        [SerializeField] private Image frameImage;

        [SerializeField] private Life.Life life;

        private List<Card> cards = new List<Card>();

        public IEnumerable<ICard> Cards => cards.Select(x => x as ICard);

        /// <summary>
        /// カードが残っているか
        /// </summary>
        public bool IsRemainCard => 0 < cards.Count;

        /// <summary>
        /// 名前を設定
        /// </summary>
        /// <param name="playerName"></param>
        public void SetName(string playerName)
        {
            nameLabel.text = playerName;
        }

        /// <summary>
        /// 残りパス回数を設定
        /// </summary>
        /// <param name="remainPassCount"></param>
        public void SetRemainPassCount(int remainPassCount)
        {
            life.SetPassCount(remainPassCount);
        }

        /// <summary>
        /// ターン開始
        /// </summary>
        public void StartTurn()
        {
            frameImage.gameObject.SetActive(true);
        }

        /// <summary>
        /// ターン終了
        /// </summary>
        public void FinishTurn()
        {
            frameImage.gameObject.SetActive(false);
        }

        /// <summary>
        /// カードを追加する
        /// </summary>
        /// <param name="card"></param>
        public void AddCard(Card card)
        {
            card.RectTransform.SetParent(transform, false);
            card.RectTransform.sizeDelta = CardUtility.CarHandSize;

            cards.Add(card);
        }

        /// <summary>
        /// カードを返却された
        /// </summary>
        /// <param name="card"></param>
        public void BackCard(Card card)
        {
            card.RectTransform.SetParent(transform, false);
            card.RectTransform.sizeDelta = CardUtility.CarHandSize;

            cards.Add(card);

            cards = cards.OrderBy(x => x.CardData.Suit).ThenBy(x => x.CardData.Number).ToList();

            var index = cards.IndexOf(card);
            card.RectTransform.SetSiblingIndex(index);
        }

        /// <summary>
        /// 手札をソートする
        /// </summary>
        public void SortHand()
        {
            SortHandAsync().Forget();
        }

        /// <summary>
        /// カードを取り除く
        /// </summary>
        /// <param name="cardData"></param>
        public Card PopCard(CardData cardData)
        {
            var card = cards.FirstOrDefault(x => x.CardData == cardData);
            cards.Remove(card);
            return card;
        }

        private async UniTaskVoid SortHandAsync()
        {
            cards = cards.OrderBy(x => x.CardData.Suit).ThenBy(x => x.CardData.Number).ToList();

            foreach (var card in cards)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.05 / MasterData.Current.GameSpeed));

                card.transform.SetAsLastSibling();
            }
        }
    }
}