using UnityEngine;
using Battle.Game;

namespace Battle
{

    public class EnemyAttack : MonoBehaviour, IEnemyState
    {
        private Enemy _enemy;
        private Animator _animator;
        private EnemyStateManager _stateManager;

        private static readonly int HashAttack = Animator.StringToHash("Attack");

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
            _animator.SetTrigger(HashAttack);
        }

        public void ExitState()
        {

        }

        public void OnUpdate()
        {

        }

        public void Attack()
        {
            Player.Instance.TakeDamage(new Damage(_enemy.EnemyParameter.ATK, gameObject, false));
        }

        public void OnAnimationAttackFinish()
        {
            _stateManager.ChangeState(EnemyStates.Idle);
        }
    }

}