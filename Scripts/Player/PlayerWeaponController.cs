using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon = Battle.Game.WeaponController;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;

namespace Battle
{
    // Playerから見た武器に関する処理を行う
    // 入力を受け取り現在の武器に伝える
    // 入力を受け取り武器を切り替える

    public class PlayerWeaponController : MonoBehaviour
    {
        // 1:InputEventの取得
        // 2:現在の武器を操作

        public Weapon CurrentWeapon => _weapons[_currentWeaponIndex];
        public IObservable<Unit> OnWeaponChanged => _changeSub = new Subject<Unit>();
        private Subject<Unit> _changeSub = new Subject<Unit>();

        private PlayerInput _input;

        [SerializeField] private Transform _root;
        [SerializeField] private List<Weapon> _weapons;
        private int _currentWeaponIndex = 0;
        private int _nextWeaponIndex = 0;

        // TODO:weaponDataというDataのみを引数にとりGameObjectとして生成する
        public void AddWeapon(Weapon weapon)
        {
            _weapons.Add(weapon);
            weapon.transform.SetParent(_root);
        }


        private void Start()
        {
            TryGetComponent(out _input);

            foreach (Transform child in _root)
            {
                var weapon = child.GetComponent<Weapon>();
                _weapons.Add(weapon);
            }

            LoopAsync(this.GetCancellationTokenOnDestroy()).Forget();
            _changeSub.AddTo(this);

            _changeSub.OnNext(Unit.Default);
        }

        private void Update()
        {
            UpdateShoot();

            UpdateWeponSwitch();

        }

        private void UpdateWeponSwitch()
        {


            if (Input.GetKeyDown(KeyCode.Alpha1))
                _nextWeaponIndex = 0;
            if (Input.GetKeyDown(KeyCode.Alpha2))
                _nextWeaponIndex = 1;
            if (Input.GetKeyDown(KeyCode.Alpha3))
                _nextWeaponIndex = 2;
            if (Input.GetKeyDown(KeyCode.Alpha4))
                _nextWeaponIndex = 3;
            if (Input.GetKeyDown(KeyCode.Alpha5))
                _nextWeaponIndex = 4;


            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _nextWeaponIndex = (_nextWeaponIndex + 1) % _weapons.Count;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (_nextWeaponIndex <= 0)
                    _nextWeaponIndex = _weapons.Count - 1;
                else
                    _nextWeaponIndex--;
            }

        }

        private async UniTaskVoid LoopAsync(CancellationToken token)
        {
            var currentWeapon = _weapons[_currentWeaponIndex];
            await currentWeapon.ShowWeaponAsync(token);

            while (!token.IsCancellationRequested)
            {
                if (_currentWeaponIndex != _nextWeaponIndex)
                {
                    await WeaponChange(token);
                }
                else
                {
                    await UniTask.Yield(token);
                }
            }
        }

        private async UniTask WeaponChange(CancellationToken token)
        {

            var currentWeapon = _weapons[_currentWeaponIndex];
            await currentWeapon.RemoveWeapon(token);
            currentWeapon.gameObject.SetActive(false);

            // 格納アニメーションのみだと武器切り替えがかぶるため、少し待機
            await UniTask.Delay(500, cancellationToken: token);

            var nextWeapon = _weapons[_nextWeaponIndex];
            nextWeapon.gameObject.SetActive(true);
            nextWeapon.ShowWeapon();

            _currentWeaponIndex = _nextWeaponIndex;

            _changeSub.OnNext(Unit.Default);
        }

        private void UpdateShoot()
        {
            var inputFire = _input.actions["Fire"];
            CurrentWeapon.HandleShootInputs(inputFire.IsPressed());
        }

    }

}