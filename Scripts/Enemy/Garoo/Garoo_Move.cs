using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Battle.Enemies
{

    public class Garoo_Move : BaseEnemyMove
    {
        [Serializable]
        public class MoveParameter
        {
            public float _defaultMoveSpeed = 1f;
            public float _minRandomSpeed = 0.8f, _maxRandomSpeed = 1.2f;

            [Header("îÚçsìGÇÃèÍçáÇÃÉâÉìÉ_ÉÄÇ»çÇÇ≥")]
            public float minThetaDeg = 0f, maxThetaDeg = 180f;
            public float minPhiDeg = 0f, maxPhiDeg = 360f;
        }


        [SerializeField] private MoveParameter _moveParameter;
        public GameObject navPoint;

        private NavMeshAgent _agent;
        private float _moveSpeed;
        private Vector3 _targetPos;

        public override void Initialize()
        {
            _enemy = GetComponent<Enemy>();
            TryGetComponent(out _agent);

            _moveSpeed = _moveParameter._defaultMoveSpeed * Random.Range(_moveParameter._minRandomSpeed, _moveParameter._maxRandomSpeed);
            _targetPos = _player.transform.position;
        }

        public override void OnStateEnter()
        {
            _agent.enabled = true;
            _agent.isStopped = false;
        }

        public override void OnStateExit()
        {
            _agent.isStopped = true;
            _agent.enabled = false;
        }

        public override void OnUpdate()
        {
            if (_player != null)
            {
                _agent.destination = _player.transform.position;
                //Move();
                //_navMesh.nextPosition = transform.position;
            }

            base.OnUpdate();



            //var dir = (_targetPos - transform.position).normalized;
            //transform.position += dir * _moveSpeed * Time.deltaTime;

            //transform.LookAt(_player.transform);
        }

        private void Move()
        {

            Vector3 nextPos;

            if (_agent.path.corners.Length > 2)
            {
                nextPos = _agent.path.corners[1];
            }
            else
            {
                nextPos = _targetPos;
            }
            navPoint.transform.position = nextPos;


        }

        private void OnDrawGizmos()
        {
            if (_agent && _agent.enabled)
            {
                Gizmos.color = Color.red;
                var prepos = transform.position;

                foreach (var pos in _agent.path.corners)
                {
                    Gizmos.DrawLine(prepos, pos);
                    prepos = pos;
                }
            }
        }
    }

}