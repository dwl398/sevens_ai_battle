using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sevens.View.Life
{
    public class Life : MonoBehaviour
    {
        [SerializeField] private GameObject original;

        private readonly List<Image> lives = new List<Image>();

        private void Awake()
        {
            foreach (var image in GetComponentsInChildren<Image>())
            {
                if (image.gameObject == gameObject)
                {
                    continue;
                }

                if (image.transform.childCount == 0)
                {
                    continue;
                }

                lives.Add(image);
            }
        }

        /// <summary>
        /// パス回数を設定
        /// </summary>
        /// <param name="remainPassCount"></param>
        public void SetPassCount(int remainPassCount)
        {
            if (lives.Count < remainPassCount)
            {
                var count = lives.Count;
                for (var i = 0; i < remainPassCount - count; i++)
                {
                    var image = Instantiate(original, transform).GetComponent<Image>();
                    lives.Add(image);
                }
                return;
            }

            for (var i = 0; i < lives.Count; i++)
            {
                lives[i].transform.GetChild(0).gameObject.SetActive(i < remainPassCount);
            }
        }
    }
}