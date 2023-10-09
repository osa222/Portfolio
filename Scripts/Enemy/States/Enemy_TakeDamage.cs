using UnityEngine;

namespace Battle.Enemies
{

    internal class Enemy_TakeDamage : MonoBehaviour, IEnemyState
    {

        private Animator _animator;
        private static readonly int HashTakeDamage = Animator.StringToHash("Take Damage");

        private EnemyStateManager _enemyStateManager;

        public void Init()
        {
            TryGetComponent(out _animator);
            TryGetComponent(out _enemyStateManager);
        }

        public void OnStateEnter()
        {
            _animator.SetTrigger(HashTakeDamage);
        }

        public void OnStateExit()
        {

        }

        public void OnUpdate()
        {

        }

        public void DamageAnimationFinish()
        {
            _enemyStateManager.ChangeState(EnemyStates.Idle);
        }

    }

}