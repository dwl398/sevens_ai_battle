using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sevens.Editor;
using Sevens.Model;
using Sevens.Model.Player;
using Sevens.View.Card;
using Sevens.View.Field;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sevens.Presenter
{
    public class SevensSequencer : MonoBehaviour
    {
        /// <summary>
        /// プレイヤー達
        /// </summary>
        [SerializeReference, SubclassSelector] private List<IPlayer> players = new List<IPlayer>();

        /// <summary>
        /// 手札達
        /// </summary>
        [SerializeField] private List<Hand> hands;

        /// <summary>
        /// カード置き場
        /// </summary>
        [SerializeField] private Field field;

        /// <summary>
        /// 誰の順番か
        /// </summary>
        private int currentTurn = 0;

        /// <summary>
        /// プレイヤー
        /// </summary>
        private class Pair
        {
            public IPlayer player;
            public Hand hand;
            public int passCount;
        }

        private List<Pair> pairs = new List<Pair>();

        private void Start()
        {
            StartAsync().Forget();
        }

        private async UniTaskVoid StartAsync()
        {
            InitPlayers();

            await InitGameAsync();

            await GameAsync();
        }

        /// <summary>
        /// プレイヤー初期化
        /// </summary>
        private void InitPlayers()
        {
            if (players.Count != 4)
            {
                Debug.Log("プレイヤーは4人で頼まい");
                return;
            }

            if (players.Contains(null))
            {
                Debug.LogError("プレイヤーが未設定です");
                return;
            }

            // プレイヤー順序を入れ替え
            players = players.OrderBy(x => Random.value).ToList();

            for (var i = 0; i < players.Count; i++)
            {
                // 名前を設定してペアにしてリスト化
                hands[i].SetName(players[i].Name);
                hands[i].SetRemainPassCount(MasterData.Current.PassCount);
                pairs.Add(new Pair { player = players[i], hand = hands[i], passCount = MasterData.Current.PassCount });
            }
        }

        /// <summary>
        /// ゲーム初期化
        /// </summary>
        private async UniTask InitGameAsync()
        {
            await DealCardsAsync();
        }

        /// <summary>
        /// 手札を配る
        /// </summary>
        private async UniTask DealCardsAsync()
        {
            // 手札を配る
            const int popCardCount = 48;
            for (var i = 0; i < popCardCount; i++)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.05 / MasterData.Current.GameSpeed));

                var card = field.PopCardByRandomWithoutSeven();
                pairs[i % pairs.Count].hand.AddCard(card);
            }

            // 手札をソートさせる
            pairs.ForEach(x => x.hand.SortHand());

            // ソートの演出を待機
            await UniTask.Delay(TimeSpan.FromSeconds(1 / MasterData.Current.GameSpeed));
        }

        /// <summary>
        /// ゲーム処理
        /// </summary>
        private async UniTask GameAsync()
        {
            var winnerCount = 0;
            var loserCount = 0;
            while (true)
            {
                // 全員の手札がなくなったら終了
                if (!pairs.Exists(x => x.hand.IsRemainCard))
                {
                    break;
                }

                // プレイヤーと手札を取得
                var pair = pairs[currentTurn];

                // 残りカードがないなら次のプレイヤーへ
                if (pair.hand.IsRemainCard == false)
                {
                    currentTurn = (currentTurn + 1) % pairs.Count;
                    continue;
                }

                pair.hand.StartTurn();
                // カードを選択
                var pickUpCard = pair.player.PickUpCard(field, pair.hand, pair.passCount);

                // パス指定
                if (pickUpCard == null)
                {
                    if (0 < pair.passCount)
                    {
                        // パス回数が残っているパス回数を消費
                        pair.passCount -= 1;
                        pair.hand.SetRemainPassCount(pair.passCount);
                    }
                    else
                    {
                        // パス回数が残っていない場合はカードを全てフィールドに置いて終了
                        await BurstPairAsync(pair, loserCount++);
                    }
                    // 次のプレイヤーへ
                    currentTurn = (currentTurn + 1) % pairs.Count;
                    continue;
                }

                // とりあえずフィールドに置いてみるけど先に判定はしておく
                var card = pair.hand.PopCard(pickUpCard.CardData);
                var added = field.CanAdd(card);
                field.PushCard(card);
                // 置いたカードを見の時間
                await UniTask.Delay(TimeSpan.FromSeconds(0.8 / MasterData.Current.GameSpeed));

                // 置けたかどうかをみる
                if (added == false)
                {
                    if (0 < pair.passCount)
                    {
                        // パス回数が残っているならカードを手札に返す
                        pair.passCount -= 1;
                        pair.hand.SetRemainPassCount(pair.passCount);
                        field.PopCard(card);
                        pair.hand.BackCard(card);
                        card.Failure();
                    }
                    else
                    {
                        // カードを全てフィールドに置いて終了
                        await BurstPairAsync(pair, loserCount++);
                    }
                }
                else
                {
                    // カードに成功エフェクトを出す
                    card.Success();
                    // 手札がなくなったら順位付け
                    if (pair.hand.IsRemainCard == false)
                    {
                        pair.hand.SetName(pair.player.Name + ":" + ++winnerCount + "い");
                    }
                }

                // 次のプレイヤーへ
                pair.hand.FinishTurn();
                currentTurn = (currentTurn + 1) % pairs.Count;
                await UniTask.Delay(TimeSpan.FromSeconds(0.2 / MasterData.Current.GameSpeed));
            }
        }

        /// <summary>
        /// パス回数がなくなって負ける
        /// </summary>
        /// <param name="pair"></param>
        /// <param name="loserCount"></param>
        private async UniTask BurstPairAsync(Pair pair, int loserCount)
        {
            foreach (var deadCard in pair.hand.Cards.ToList())
            {
                var pop = pair.hand.PopCard(deadCard.CardData);
                pop.Failure();
                field.PushCard(pop);
                await UniTask.Delay(TimeSpan.FromSeconds(0.1 / MasterData.Current.GameSpeed));
            }

            // 順位をつけてあげる
            pair.hand.SetName(pair.player.Name + ":" + (4 - loserCount) + "い");
            pair.hand.FinishTurn();
            await UniTask.Delay(TimeSpan.FromSeconds(0.8 / MasterData.Current.GameSpeed));
        }
    }
}