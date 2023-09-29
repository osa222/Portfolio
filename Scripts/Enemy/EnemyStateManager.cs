using System.Collections.Generic;
using UnityEngine;

namespace Battle
{

    public interface IEnemyState
    {
        void EnterState();
        void ExitState();
        void OnUpdate();
    }

    public enum EnemyStates
    {
        Idle,
        Attack,
        Follow,
        None
    }

    // このクラスは、敵の基本的な「AI」を制御します。これは、次の 3 つの状態の間で有限状態
    // マシンとして機能します。 - 攻撃 - 追跡/追跡 - アイドル
    public class EnemyStateManager : MonoBehaviour
    {
        [SerializeField] private string _playerTag = "Player";
        public GameObject Player;

        private Enemy _enemy;
        private List<IEnemyState> _states;
        private IEnemyState _currentStateHandler;
        private EnemyStates _currentState = EnemyStates.None;

        private void Start()
        {
            Player = GameObject.FindGameObjectWithTag(_playerTag);

            _enemy = GetComponent<Enemy>();
            _states = new List<IEnemyState>
            {
                GetComponent<EnemyIdle>(),
                GetComponent<EnemyAttack>(),
                GetComponent<EnemyMovement>()
            };

            ChangeState(EnemyStates.Follow);
        }


        public void ChangeState(EnemyStates state)
        {
            if (_currentState == state)
                return;

            if (_currentStateHandler != null)
            {
                _currentStateHandler.ExitState();
                _currentStateHandler = null;
            }

            _currentState = state;
            _currentStateHandler = _states[(int)state];
            _currentStateHandler.EnterState();
        }

        private void Update()
        {
            if (_enemy.IsAlive)
                _currentStateHandler.OnUpdate();
        }

    }
}