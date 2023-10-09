using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Battle.Enemies
{


    public class ZigZagMoveing : BaseEnemyMove
    {

        [Serializable]
        public class MoveParameter
        {
            public float defaultMoveSpeed = 5f;
            public float minRandomSpeedMultiplier = 0.8f, maxRandomSpeedMultiplier = 1.2f;

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

        public override void Initialize()
        {
            _moveSpeed = _moveParameter.defaultMoveSpeed * Random.Range(_moveParameter.minRandomSpeedMultiplier, _moveParameter.maxRandomSpeedMultiplier);
            _previousPos = transform.position;
        }


        private void ZigZagMove()
        {
            Vector3 targetDirection = (_player.transform.position - transform.position).normalized;
            float zigzagOffset = Mathf.Sin(Time.time * _moveParameter.zigzagFrequency) * _moveParameter.zigzagMagnitude;
            Vector3 orthogonalVector = new Vector3(-targetDirection.z, 0f, targetDirection.x);
            Vector3 moveDirection = (targetDirection + orthogonalVector * zigzagOffset).normalized;
            transform.Translate(moveDirection * _moveSpeed * Time.deltaTime, Space.World);

            var dir = (transform.position - _previousPos).normalized;
            var rotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 7f * Time.deltaTime);

            LookAtPlayer();
            _previousPos = transform.position;
        }

        [SerializeField] private float _rotationSpeed = 3;


        private void LookAtPlayer()
        {
            //現在のオブジェクトの進行方向を取得
            Vector3 forwardDirection = transform.forward;
            // ターゲットオブジェクトへのベクトルを取得
            Vector3 targetDirection = (_player.transform.position - transform.position).normalized;
            var rotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
        }

        public override void OnStateEnter()
        {
        }

        public override void OnUpdate()
        {
            ZigZagMove();
        }

        public override void OnStateExit()
        {
        }
    }

}