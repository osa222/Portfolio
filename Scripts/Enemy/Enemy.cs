using UnityEngine;
using Battle.Game;
using UnityEngine.Events;

namespace Battle
{
    public class Enemy : MonoBehaviour
    {
        public EnemyParameter EnemyParameter => _enemyParameter;
        [SerializeField] private EnemyParameter _enemyParameter;

        public UnityEvent<Damage> OnDamage;

        public bool IsAlive => _isAlive;
        private bool _isAlive = true;

        public int MaxHP => _enemyParameter.MAXHP;
        public int HP => _health;
        private int _health;


        [SerializeField] private GameObject _vfx;

        private Animator _animator;
        private EnemyStateManager _enemyStateManager;

        private static readonly int HashTakeDamage = Animator.StringToHash("Take Damage");
        private static readonly int HashDie = Animator.StringToHash("Die");

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _health = EnemyParameter.MAXHP;
            _animator = GetComponent<Animator>();
            _enemyStateManager = GetComponent<EnemyStateManager>();
        }

        public void TakeDamage(Damage damage)
        {
            if (!_isAlive) return;

            _health -= damage.Value;

            _animator.SetTrigger(HashTakeDamage);

            OnDamage.Invoke(damage);

            if (_health <= 0 && _isAlive)
            {
                Die();
            }
        }

        public void DamageAnimationFinish()
        {
            _enemyStateManager.ChangeState(EnemyStates.Idle);
        }

        private void Die()
        {
            _isAlive = false;
            _animator.SetTrigger(HashDie);

            var effectObj = Instantiate(_vfx, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            Destroy(effectObj.gameObject, effectObj.main.duration);

            Destroy(this.gameObject, 0.5f);

            if (BattleSystem.Instance != null)
            {
                BattleSystem.Instance.KillCount++;
            }

            // SE,Effectçƒê∂
        }
    }

}
