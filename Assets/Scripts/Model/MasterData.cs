using UnityEngine;

namespace Sevens.Model
{
    [CreateAssetMenu(fileName = nameof(MasterData), menuName = "ScriptableObject/" + nameof(MasterData))]
    public class MasterData : ScriptableObject
    {
        private static MasterData current;

        public static MasterData Current
        {
            get
            {
                if (current == null)
                {
                    current = Resources.Load<MasterData>(nameof(MasterData));
                }

                return current;
            }
        }

        /// <summary>
        /// ゲームスピード
        /// </summary>
        [Range(0.5f, 3.0f)] public float GameSpeed = 1.0f;

        /// <summary>
        /// パス回数
        /// </summary>
        [Range(1, 10)] public int PassCount = 4;
    }
}