using UnityEngine;
using Random = UnityEngine.Random;

namespace Battle
{
    //近距離・遠距離・ボスの３分類がある
    public class EnemyMovement : MonoBehaviour, IEnemyState
    {
        [SerializeField] private string _playerTag = "Player";

        private BaseEnemyMove _currentMove;
        private GameObject _player;

        public Vector3 Velocity { get; private set; }
        private Vector3 _prevPosition;
        private Animator _animator;
        private EnemyStateManager _stateManager;
        private Enemy _enemy;

        private static readonly int HashIsMove = Animator.StringToHash("IsMove");

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag(_playerTag);
            Initialize();
        }

        private void Initialize()
        {
            _enemy = GetComponent<Enemy>();
            _animator = GetComponent<Animator>();
            _currentMove = GetComponent<BaseEnemyMove>();
            _stateManager = GetComponent<EnemyStateManager>();

            _currentMove.OnStart();
        }

        public void EnterState()
        {

        }

        public void ExitState()
        {
            _currentMove.OnStateExit();
        }

        public void OnUpdate()
        {
            _currentMove.OnStateUpdate();

            if (Mathf.Approximately(Time.deltaTime, 0))
                return;

            Velocity = (transform.position - _prevPosition) / Time.deltaTime;
            SetAnimatorParameter();
            _prevPosition = transform.position;

            if (Vector3.Distance(transform.position, _player.transform.position) <= _enemy.EnemyParameter.AttackRange)
            {
                transform.LookAt(_player.transform);
                _stateManager.ChangeState(EnemyStates.Attack);
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


    public abstract class BaseEnemyMove : MonoBehaviour
    {
        [SerializeField] protected string _playerTag = "Player";

        protected GameObject _player;
        protected Enemy _enemy;

        public void OnStart()
        {
            _enemy = GetComponent<Enemy>();
            _player = GameObject.FindGameObjectWithTag(_playerTag);

            Initialize();
            OnStateEnter();
        }

        public abstract void Initialize();
        public abstract void OnStateEnter();
        public abstract void OnStateUpdate();
        public abstract void OnStateExit();
    }


}