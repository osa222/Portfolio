using System;
using System.Threading;
using System.Collections;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

namespace Battle.Game
{

    public class WeaponController : MonoBehaviour
    {
        enum WeaponState
        {
            Idling,
            Preparing,
            Reloading,
        }

        [Serializable]
        public class Settings
        {
            public BaseBullet bulletPrefab;
            public Transform weaponMuzzle;
            public float bulletSpreadAngle;
        }

        public WeaponData _parameter;
        public Settings _settings;
        public Weapon Weapon;

        private WeaponState _state = WeaponState.Idling;

        public IReadOnlyReactiveProperty<int> CurrentAmmo => _currentAmmo;
        private ReactiveProperty<int> _currentAmmo = new ReactiveProperty<int>(int.MaxValue);

        private float _currentRecastTimer;

        private Animator _animator;
        private Camera _cam;

        private Subject<Unit> _removeSub = new Subject<Unit>();
        private Subject<Unit> _importSub = new Subject<Unit>();


        private void Awake()
        {
            _removeSub.AddTo(this);
            _importSub.AddTo(this);
            _currentAmmo.AddTo(this);

            _currentAmmo.Value = _parameter.magazineSize;

            _cam = Camera.main;
            TryGetComponent(out Weapon);
            TryGetComponent(out _animator);

            _animator.keepAnimatorControllerStateOnDisable = true;
        }

        private void OnEnable()
        {
            _state = WeaponState.Idling;
            _animator.SetBool("IsReload", false);
        }


        private bool _inputDown;

        /// <summary>
        /// 武器の入力をとる
        /// </summary>
        public bool HandleShootInputs(bool inputDown)
        {
            if (_state == WeaponState.Idling && inputDown)
            {
                _inputDown = inputDown;
                return true;
            }
            else
                return false;
        }

        // 武器の発射→リロードの流れを処理する
        private IEnumerator LoopCoroutine()
        {
            if (_currentAmmo.Value <= 0)
                yield return Reload();

            while (true)
            {

                yield return null;

                if (!_inputDown)
                {
                    continue;
                }

                _inputDown = false;
                yield return Shoot();


                // 弾がゼロの場合リロード
                if (_currentAmmo.Value <= 0)
                    yield return Reload();

            }

        }

        [SerializeField] private float _objectDistance;
        private IEnumerator Shoot()
        {
            if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out var hit, _objectDistance))
            {
                _settings.weaponMuzzle.LookAt(hit.point);
            }
            else
            {
                _settings.weaponMuzzle.LookAt(_cam.transform.position + (_cam.transform.forward * 50f));
            }

            // 弾を発射する
            var shotDir = GetShotDirectionWithinSpread(_settings.weaponMuzzle);
            var bullet = Instantiate(_settings.bulletPrefab, _settings.weaponMuzzle.position, Quaternion.LookRotation(shotDir));
            bullet.Shoot(new Damage(_parameter.damage, this.gameObject, false));
            _currentAmmo.Value--;
            _animator.SetTrigger("Fire");

            _state = WeaponState.Preparing;

            // 発射間隔秒待機
            yield return new WaitForSeconds(_parameter.shootInterval);

            _state = WeaponState.Idling;
        }

        private IEnumerator Reload()
        {
            _state = WeaponState.Reloading;
            _animator.SetBool("IsReload", true);

            yield return new WaitForSeconds(_parameter.reloadTime);

            _animator.SetBool("IsReload", false);

            Debug.Log("Reloading...");

            _state = WeaponState.Idling;
            _currentAmmo.Value = _parameter.magazineSize;
        }

        public Vector3 GetShotDirectionWithinSpread(Transform shootTransform)
        {
            float spreadAngleRatio = _settings.bulletSpreadAngle / 180f;
            Vector3 spreadWorldDirection = Vector3.Slerp(shootTransform.forward, UnityEngine.Random.insideUnitSphere,
                spreadAngleRatio);

            return spreadWorldDirection;
        }

        public void ShowWeapon()
        {
            ShowWeaponAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        public async UniTask ShowWeaponAsync(CancellationToken token)
        {
            _state = WeaponState.Preparing;

            // 出現アニメーションの待機
            await _importSub.FirstOrDefault().ToUniTask(cancellationToken: token);
            _state = WeaponState.Idling;


            StartCoroutine(LoopCoroutine());
        }

        public async UniTask RemoveWeapon(CancellationToken token)
        {
            _animator.SetTrigger("Remove");
            await _removeSub.FirstOrDefault().ToUniTask(cancellationToken: token);
        }

        #region AnimationEvent
        public void CompleteImport()
        {
            _importSub.OnNext(Unit.Default);
        }

        public void CompleteRemove()
        {
            _removeSub.OnNext(Unit.Default);
        }
        #endregion
    }

}