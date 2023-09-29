using System;
using System.Threading;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Battle
{

    public class RandomMoveing : BaseEnemyMove
    {


        public RandomMove[] _randomMoves;
        private CancellationTokenSource _cts;
        public override void Initialize()
        {
            foreach (var move in _randomMoves) move.Init();
        }

        private IEnumerator RandomMove(CancellationToken token)
        {

            while (!token.IsCancellationRequested)
            {
                var index = Random.Range(0, _randomMoves.Length);
                var randomMove = _randomMoves[index];

                // ランダムに選択された行動を3秒間の間行う
                float timer = 0;
                randomMove.OnStateEnter();
                while (timer <= 3f && !token.IsCancellationRequested)
                {
                    randomMove.OnStateUpdate();
                    yield return null;
                    timer += Time.deltaTime;
                }
                randomMove.OnStateExit();

            }
        }

        public override void OnStateEnter()
        {
            _cts = new CancellationTokenSource();
            StartCoroutine(RandomMove(_cts.Token));
        }

        public override void OnStateUpdate()
        {

        }

        public override void OnStateExit()
        {
            _cts.Cancel();
        }
    }

    public abstract class RandomMove : MonoBehaviour
    {
        public abstract void Init();
        public abstract void OnStateEnter();
        public abstract void OnStateUpdate();
        public abstract void OnStateExit();
    }
}