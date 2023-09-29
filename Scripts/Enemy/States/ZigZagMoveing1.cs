using System;
using System.Threading;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Battle
{


    public class ZigZagMoveing1 : BaseEnemyMove
    {

        [Serializable]
        public class MoveParameter
        {
            public float defaultMoveSpeed = 5f;
            public float minRandomSpeed = 0.8f, maxRandomSpeed = 1.2f;

            [Header("何秒かけてPlayerの方を向くか")]
            public float rotationTime = 1f;

            [Header("ジグザグの周波数（1秒あたりのジグザグ回数）")]
            public float zigzagFrequency = 2f;

            [Header("ジグザグの振れ幅")]
            public float zigzagMagnitude = 1f;
        }

        [SerializeField] private MoveParameter _moveParameter;
        private float _moveSpeed;
        private Vector3 _previousPos;
        private CancellationTokenSource _cts;


        public override void Initialize()
        {
            _moveSpeed = _moveParameter.defaultMoveSpeed * Random.Range(_moveParameter.minRandomSpeed, _moveParameter.maxRandomSpeed);
        }

        private IEnumerator MoveCoroutine(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                yield return RotateCoroutine();
                yield return new WaitForSeconds(0.5f);

                float timer = 0f;
                while (timer <= 3f && !token.IsCancellationRequested)
                {
                    ZigZagMove();
                    timer += Time.deltaTime;
                    yield return null;
                }
            }
        }

        private void ZigZagMove()
        {


            // ターゲットに向かう方向を求める
            Vector3 targetDirection = (_player.transform.position - transform.position).normalized;

            // ジグザグの動きを計算
            float zigzagOffset = Mathf.Sin(Time.time * _moveParameter.zigzagFrequency) * _moveParameter.zigzagMagnitude;

            // ターゲット方向と直交するベクトルを計算
            Vector3 orthogonalVector = new Vector3(-targetDirection.z, 0f, targetDirection.x);

            // ジグザグの動きに直交するベクトルを足して、移動方向を求める
            Vector3 moveDirection = (targetDirection + orthogonalVector * zigzagOffset).normalized;

            // 移動処理
            transform.Translate(moveDirection * _moveSpeed * Time.deltaTime, Space.World);


            // 進行方向を向く
            var dir = (transform.position - _previousPos).normalized;
            var rotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 7f * Time.deltaTime);

            _previousPos = transform.position;
        }

        private IEnumerator RotateCoroutine()
        {
            var initialRotation = transform.rotation;
            var dir = (_player.transform.position - transform.position).normalized;
            var playerRotation = Quaternion.LookRotation(dir);

            float elapsedTime = 0f;
            while (elapsedTime <= _moveParameter.rotationTime)
            {
                elapsedTime += Time.deltaTime;
                var t = Mathf.Clamp01(elapsedTime / _moveParameter.rotationTime);
                transform.rotation = Quaternion.Slerp(initialRotation, playerRotation, t);
                yield return null;
            }
        }

        public override void OnStateEnter()
        {
            _cts = new CancellationTokenSource();
            StartCoroutine(MoveCoroutine(_cts.Token));
        }

        public override void OnStateUpdate()
        {
        }

        public override void OnStateExit()
        {
            _cts.Cancel();
        }
    }
}