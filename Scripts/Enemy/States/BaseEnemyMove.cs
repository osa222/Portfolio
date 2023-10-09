using UnityEngine;

namespace Battle.Enemies
{

    public abstract class BaseEnemyMove : MonoBehaviour, IEnemyState
    {
        protected GameObject _player;
        [SerializeField] private string _playerTag = "Player";

        protected Enemy _enemy;
        private EnemyStateManager _stateManager;
        private Animator _animator;
        private static readonly int HashIsMove = Animator.StringToHash("IsMove");

        public Vector3 Velocity { get; private set; }
        private Vector3 _prevPosition;

        public void Init()
        {
            _player = GameObject.FindGameObjectWithTag(_playerTag);

            _enemy = GetComponent<Enemy>();
            _animator = GetComponent<Animator>();
            _stateManager = GetComponent<EnemyStateManager>();

            Initialize();
        }

        public abstract void Initialize();
        public virtual void OnStateEnter()
        {

        }

        public virtual void OnStateExit()
        {
        }

        public virtual void OnUpdate()
        {

            if (Mathf.Approximately(Time.deltaTime, 0))
                return;

            Velocity = (transform.position - _prevPosition) / Time.deltaTime;
            SetAnimatorParameter();
            _prevPosition = transform.position;

            if (Vector3.Distance(transform.position, _player.transform.position) <= _enemy.EnemyParameter.AttackRange)
            {
                transform.LookAt(_player.transform);
                _stateManager.ChangeState(EnemyStates.Attack);
                return;
            }
        }

        private void SetAnimatorParameter()
        {
            var hasHorizontalInput = !Mathf.Approximately(Velocity.x, 0f);
            var hasVerticalInput = !Mathf.Approximately(Velocity.z, 0f);
            var isWalking = hasHorizontalInput || hasVerticalInput;
            _animator.SetBool(HashIsMove, isWalking);
        }
    }

}