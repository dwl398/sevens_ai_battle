using System.Collections.Generic;
using Sevens.Model.Card;
using Sevens.Utility;
using UnityEngine;

namespace Sevens.View.Card
{
    /// <summary>
    /// カード配置するくん
    /// </summary>
    public class CardsCreator : MonoBehaviour
    {
        [SerializeField] private Card cardPrefab;

        private readonly List<Card> cards = new List<Card>();

        [ContextMenu("CreateCards")]
        private void CreateCards()
        {
            ClearCards();

            for (var suit = CardSuit.Spade; suit <= CardSuit.Club; suit++)
            {
                for (var number = CardNumber.Ace; number <= CardNumber.King; number++)
                {
                    var data = new CardData(suit, number);
                    var card = Instantiate(cardPrefab, transform);
                    card.name = "Card:" + data.Suit.ToString() + " " + data.Number.ToString();
                    card.SetCardData(data);
                    cards.Add(card);

                    card.GetComponent<RectTransform>().anchoredPosition = CardUtility.GetPositionByCard(card);
                }
            }
        }

        private void ClearCards()
        {
            cards.Clear();
            cards.AddRange(GetComponentsInChildren<Card>());

            foreach (var card in cards)
            {
                DestroyImmediate(card.gameObject);
            }

            cards.Clear();
        }
    }
}