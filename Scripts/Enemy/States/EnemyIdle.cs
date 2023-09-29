using UnityEngine;

namespace Battle
{

    public class EnemyIdle : MonoBehaviour, IEnemyState
    {
        private Enemy _enemy;
        private Animator _animator;
        private EnemyStateManager _stateManager;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _enemy = GetComponent<Enemy>();
            _animator = GetComponent<Animator>();
            _stateManager = GetComponent<EnemyStateManager>();
        }

        public void EnterState()
        {
            //  _animator.SetTrigger("Idol");
        }

        public void ExitState()
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
