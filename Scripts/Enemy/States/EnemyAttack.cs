using UnityEngine;
using Battle.Game;

namespace Battle.Enemies
{

    internal class EnemyAttack : MonoBehaviour, IEnemyState
    {
        private Enemy _enemy;
        private Animator _animator;
        private EnemyStateManager _stateManager;

        private static readonly int HashAttack = Animator.StringToHash("Attack");

        public void Init()
        {
            _enemy = GetComponent<Enemy>();
            _animator = GetComponent<Animator>();
            _stateManager = GetComponent<EnemyStateManager>();
        }

        public void OnStateEnter()
        {
            _animator.SetTrigger(HashAttack);
        }

        public void OnStateExit()
        {

        }

        public void OnUpdate()
        {

        }

        public void Attack()
        {
            Player.Instance.TakeDamage(new Damage(_enemy.EnemyParameter.ATK, gameObject, false));
        }

        public void OnFinishAttackAnimation()
        {
            _stateManager.ChangeState(EnemyStates.Idle);
        }
    }

}