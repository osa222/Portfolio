using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Battle.Enemies
{

    public class LinearMovement : BaseEnemyMove
    {
        [Serializable]
        public class MoveParameter
        {
            public float DefaultMoveSpeed = 1f;
            public float MinRandomSpeed = 0.8f, MaxRandomSpeed = 1.2f;

            [Header("飛行敵の場合のランダムな高さ")]
            public float MinThetaDeg = 0f, MaxThetaDeg = 180f;
            public float MinPhiDeg = 0f, MaxPhiDeg = 360f;
        }

        [SerializeField] private MoveParameter _moveParameter;
        private Vector3 _targetPos;
        private float _moveSpeed;

        public override void Initialize()
        {

            _moveSpeed = _moveParameter.DefaultMoveSpeed * Random.Range(_moveParameter.MinRandomSpeed, _moveParameter.MaxRandomSpeed);

            if (_enemy.EnemyParameter.Type == EnemyType.Fly)
            {
                // 飛行種の敵の場合ゴール位置をランダムな高さにする
                _targetPos = _player.transform.position + GetRandomPointOnSphere();
            }
            else
            {
                _targetPos = _player.transform.position;
            }
        }

        private Vector3 GetRandomPointOnSphere()
        {
            float minThetaRad = _moveParameter.MinThetaDeg * (Mathf.PI / 180f);
            float maxThetaRad = _moveParameter.MaxThetaDeg * (Mathf.PI / 180f);
            float minPhiRad = _moveParameter.MinPhiDeg * (Mathf.PI / 180f);
            float maxPhiRad = _moveParameter.MaxPhiDeg * (Mathf.PI / 180f);

            float theta = Random.Range(minThetaRad, maxThetaRad);        // 極角 (0〜π)
            float phi = Random.Range(minPhiRad, maxPhiRad);     // 方位角 (0〜2π)

            float x = _enemy.EnemyParameter.AttackRange * Mathf.Sin(theta) * Mathf.Cos(phi);
            float y = _enemy.EnemyParameter.AttackRange * Mathf.Sin(theta) * Mathf.Sin(phi);
            float z = _enemy.EnemyParameter.AttackRange * Mathf.Cos(theta);

            return new Vector3(x, y, z);
        }

        public override void OnStateEnter()
        {
            // 初期化時にすでに設定済み
        }

        public override void OnUpdate()
        {
            var dir = (_targetPos - transform.position).normalized;
            transform.position += dir * _moveSpeed * Time.deltaTime;

            transform.LookAt(_player.transform);

            base.OnUpdate();
        }

        public override void OnStateExit()
        {
            // 何か必要ならば
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawIcon(_targetPos, "targetPos");
        }
    }

}
