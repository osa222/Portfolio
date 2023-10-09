using UnityEngine;

namespace Battle.Enemies
{

    internal class EnemyIdle : MonoBehaviour, IEnemyState
    {
        private Enemy _enemy;
        private Animator _animator;
        private EnemyStateManager _stateManager;

        public void Init()
        {
            _enemy = GetComponent<Enemy>();
            _animator = GetComponent<Animator>();
            _stateManager = GetComponent<EnemyStateManager>();
        }

        public void OnStateEnter()
        {
            //  _animator.SetTrigger("Idol");
        }

        public void OnStateExit()
        {

        }

        public void OnUpdate()
        {
            if (Vector3.Distance(transform.position, _stateManager.Player.transform.position) <= _enemy.EnemyParameter.AttackRange)
            {
                transform.LookAt(_stateManager.Player.transform);
                _stateManager.ChangeState(EnemyStates.Attack);
            }
            else
            {
                _stateManager.ChangeState(EnemyStates.Follow);
            }
        }

    }

}
