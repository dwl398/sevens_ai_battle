using System.Text;
using Cysharp.Threading.Tasks;
using Sevens.Model;
using Sevens.Model.Card;
using UnityEngine;
using UnityEngine.UI;

namespace Sevens.View.Card
{
    public class Card : MonoBehaviour, ICard
    {
        [SerializeField] private CardData cardData;
        public CardData CardData => cardData;

        public RectTransform RectTransform { get; private set; }

        private Image image;

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            image = GetComponent<Image>();
        }

        /// <summary>
        /// 成功
        /// </summary>
        public void Success()
        {
            ColorEffectAsync(Color.blue).Forget();
        }

        /// <summary>
        /// 失敗
        /// </summary>
        public void Failure()
        {
            ColorEffectAsync(Color.red).Forget();
        }

        /// <summary>
        /// 色効果演出
        /// </summary>
        private async UniTaskVoid ColorEffectAsync(Color color)
        {
            var startColor = Color.white;
            var time = 0.0f;

            while (true)
            {
                time += Time.deltaTime * MasterData.Current.GameSpeed;
                var t = Mathf.Sin(time * Mathf.PI);
                var p = Mathf.Clamp01(t / 0.3f);
                var c = Color.Lerp(startColor, color, p);

                image.color = c;

                await UniTask.Yield();

                if(1.0f <= time)
                {
                    break;
                }
            }
        }

#if UNITY_EDITOR
        private CardData previousCardData = new CardData();

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                return;
            }

            ApplyCardData();
        }

        public void SetCardData(CardData data)
        {
            cardData = data;
            ApplyCardData();
        }

        private void ApplyCardData()
        {
            if (cardData == previousCardData)
            {
                return;
            }

            previousCardData = new CardData(cardData);
            var path = GetAssetPath(cardData);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(path);
            var image = GetComponent<Image>();
            image.sprite = sprite;
        }

        private string GetAssetPath(CardData data)
        {
            if (data.Suit == CardSuit.None || data.Number == CardNumber.None)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            sb.Append("card_");
            sb.Append(data.Suit.ToString().ToLower());
            sb.Append("_");
            sb.Append(((int)data.Number).ToString("D2"));

            return "Assets/Images/Sprites/Card/" + sb.ToString() + ".png";
        }
#endif
    }
}