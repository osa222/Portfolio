using UnityEngine;
using UniRx;
using Battle.Game;
using System;

namespace Battle
{

    public class Player : MonoBehaviour
    {
        #region CreateSingleton
        public static Player Instance;

        private void Awake()
        {
            Instance = this;
        }
        #endregion;

        public enum HealthState { Healthy, Dying, Dead }
        public IReadOnlyReactiveProperty<int> CurrentHP => _hp;
        public IReadOnlyReactiveProperty<HealthState> CurrentHealthState => _stateRP;

        public IObservable<Damage> OnDamage => _damageSub;
        private Subject<Damage> _damageSub = new Subject<Damage>();


        public int MaxHP => maxHP;

        [SerializeField] private int maxHP = 100;

        private bool _isAllive = true;
        private ReactiveProperty<int> _hp = new ReactiveProperty<int>();
        private ReactiveProperty<HealthState> _stateRP = new ReactiveProperty<HealthState>(HealthState.Healthy);

        private void Start()
        {
            _damageSub.AddTo(this);
            _stateRP.AddTo(this);
            _hp.AddTo(this);

            _hp.Value = maxHP;
        }

        public void TakeDamage(Damage damage)
        {
            _damageSub.OnNext(damage);

            if (!_isAllive) return;

            _hp.Value = Mathf.Clamp(_hp.Value - damage.Value, 0, maxHP);

            _stateRP.Value = _hp.Value >= 20 ? HealthState.Healthy : HealthState.Dying;


            if (_hp.Value <= 0) _isAllive = false;
        }
    }

}