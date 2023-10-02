using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Battle
{
    // 近距離・遠距離・ボスの３分類があるが同一AIとあったが、仕様が固まっていくうちに、敵ごとに細かな挙動が変わることが予想されたため、クラスを分けた
    public class Garoo_Move : BaseEnemyMove
    {
        [Serializable]
        public class MoveParameter
        {
            public float _defaultMoveSpeed = 1f;
            public float _minRandomSpeed = 0.8f, _maxRandomSpeed = 1.2f;

            [Header("飛行敵の場合のランダムな高さ")]
            public float minThetaDeg = 0f, maxThetaDeg = 180f;
            public float minPhiDeg = 0f, maxPhiDeg = 360f;
        }


        [SerializeField] private MoveParameter _moveParameter;
        public GameObject navPoint;

        private NavMeshAgent _navMesh;
        private float _moveSpeed;
        private Vector3 _targetPos;

        public override void Initialize()
        {
            _enemy = GetComponent<Enemy>();
            TryGetComponent(out _navMesh);

            _moveSpeed = _moveParameter._defaultMoveSpeed * Random.Range(_moveParameter._minRandomSpeed, _moveParameter._maxRandomSpeed);
            _targetPos = _player.transform.position;
        }

        public override void OnStateEnter()
        {

        }

        public override void OnStateUpdate()
        {
            if (_player != null)
            {
                _navMesh.destination = _player.transform.position;
                //Move();
                //_navMesh.nextPosition = transform.position;
            }



            //var dir = (_targetPos - transform.position).normalized;
            //transform.position += dir * _moveSpeed * Time.deltaTime;

            //transform.LookAt(_player.transform);
        }


        public override void OnStateExit()
        {
            _navMesh.isStopped = true;
        }


        private void Move()
        {

            Vector3 nextPos;

            if (_navMesh.path.corners.Length > 2)
            {
                nextPos = _navMesh.path.corners[1];
            }
            else
            {
                nextPos = _targetPos;
            }
            navPoint.transform.position = nextPos;


        }

        private void OnDrawGizmos()
        {
            if (_navMesh && _navMesh.enabled)
            {
                Gizmos.color = Color.red;
                var prepos = transform.position;

                foreach (var pos in _navMesh.path.corners)
                {
                    Gizmos.DrawLine(prepos, pos);
                    prepos = pos;
                }
            }
        }
    }

}