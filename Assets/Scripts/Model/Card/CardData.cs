using System;
using UnityEngine;

namespace Sevens.Model.Card
{
    /// <summary>
    /// カード情報
    /// </summary>
    [Serializable]
    public class CardData : IEquatable<CardData>
    {
        [SerializeField] private CardSuit suit = CardSuit.None;
        [SerializeField] private  CardNumber number = CardNumber.None;

        public CardSuit Suit => suit;
        public CardNumber Number => number;

        public CardData()
        {
        }

        public CardData(CardData data)
        {
            suit = data.suit;
            number = data.number;
        }

        public CardData(CardSuit cardSuit, CardNumber cardNumber)
        {
            suit = cardSuit;
            number = cardNumber;
        }

        public bool Equals(CardData other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return suit == other.suit && number == other.number;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((CardData)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)suit * 397) ^ (int)number;
            }
        }

        public static bool operator ==(CardData left, CardData right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CardData left, CardData right)
        {
            return !Equals(left, right);
        }
    }
}