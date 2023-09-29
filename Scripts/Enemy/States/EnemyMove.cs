using System.Collections;
using System.Threading;
using UnityEngine;

namespace Battle.GamePlay
{

    public class EnemyMove : BaseEnemyMove
    {
        private CancellationTokenSource _cts;

        public override void Initialize()
        {

        }

        public override void OnStateEnter()
        {
            _cts = new CancellationTokenSource();
            StartCoroutine(MoveLoop(_cts.Token));
        }

        public override void OnStateUpdate()
        {

        }

        public override void OnStateExit()
        {
            _cts.Cancel();
        }

        private IEnumerator MoveLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var randomValue = Random.Range(0, 100);

                if (randomValue <= 40)
                {

                    yield return LinearMove(1f, token);
                }
                else
                {
                    var deg = Random.Range(-60, 60);
                    var moveDir = (_player.transform.position - transform.position).normalized;
                    var newDir = Quaternion.Euler(0, deg, 0) * moveDir;

                    yield return RotateCoroutine(newDir, 0.5f, token);

                    yield return Move(newDir, 1f, token);
                }

                // yield return null;
            }
        }

        private IEnumerator LinearMove(float delay, CancellationToken token)
        {
            var timer = 0f;
            while (timer <= delay && !token.IsCancellationRequested)
            {
                timer += Time.deltaTime;

                var moveDir = (_player.transform.position - transform.position).normalized;
                transform.position += moveDir * _speed * Time.deltaTime;

                var rotation = Quaternion.LookRotation(moveDir);
                _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, rotation, 7f * Time.deltaTime);
                yield return null;
            }
        }

        private float _speed = 3f;

        private IEnumerator Move(Vector3 newDir, float delay, CancellationToken token)
        {
            var rotation = Quaternion.LookRotation(newDir);
            var timer = 0f;

            while (timer <= delay && !token.IsCancellationRequested)
            {
                timer += Time.deltaTime;

                _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, rotation, 7f * Time.deltaTime);
                transform.position += newDir * _speed * Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator RotateCoroutine(Vector3 newDir, float delay, CancellationToken token)
        {
            var rotation = Quaternion.LookRotation(newDir);

            var initRotation = transform.rotation;
            float rotationTimer = 0f;

            while (rotationTimer <= delay && !token.IsCancellationRequested)
            {

                rotationTimer += Time.deltaTime;

                var t = rotationTimer / delay;

                transform.rotation = Quaternion.Slerp(initRotation, rotation, t);
                yield return null;
            }
        }
    }

}