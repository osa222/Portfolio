using System;
using System.Threading;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

using UniRx;
using Cysharp.Threading.Tasks;

namespace Battle.Game
{

    public class WeaponController : MonoBehaviour
    {

        enum State
        {
            射撃可能,
            射撃不可,
            リロード中,
            リキャスト中
        }

        [Serializable]
        public class Settings
        {
            public BaseBullet _bulletPrefab;
            public Transform WeaponMuzzle;
            public Transform FollowTarget;
            public Transform LookTarget;
            public float BulletSpreadAngle;
        }

        public WeaponData _parameter;
        public Settings _settings;
        public Weapon Weapon;

        private State _state = State.射撃可能;

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
            _state = State.射撃可能;
            _animator.SetBool("IsReload", false);
        }


        public bool HandleShootInputs(bool inputDown)
        {
            if (_state == State.射撃可能 && inputDown)
            {
                Shoot();
                return true;
            }
            else
                return false;
        }

        private void Update()
        {
            if (_state == State.リロード中) return;

            if (_currentAmmo.Value <= 0)
            {
                StartCoroutine(Reload());
                return;
            }

            if (_state == State.リキャスト中)
                Recast();
        }

        private void Recast()
        {
            _currentRecastTimer += Time.deltaTime;
            if (_currentRecastTimer >= _parameter.shootInterval)
            {
                _state = State.射撃可能;
                _currentRecastTimer = 0f;
            }
        }


        [SerializeField] private float _objectDistance;
        private void Shoot()
        {
            if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out var hit, _objectDistance))
            {
                _settings.WeaponMuzzle.LookAt(hit.point);
            }
            else
            {
                _settings.WeaponMuzzle.LookAt(_cam.transform.position + (_cam.transform.forward * 50f));
            }

            // 弾を発射する
            var shotDir = GetShotDirectionWithinSpread(_settings.WeaponMuzzle);
            var bullet = Instantiate(_settings._bulletPrefab, _settings.WeaponMuzzle.position, Quaternion.LookRotation(shotDir));
            bullet.Shoot(new Damage(_parameter.damage, this.gameObject, false));
            _currentAmmo.Value--;
            _animator.SetTrigger("Fire");

            _state = State.リキャスト中;

        }

        private IEnumerator Reload()
        {
            _state = State.リロード中;
            _animator.SetBool("IsReload", true);
            yield return new WaitForSeconds(_parameter.ReloadTime);
            _animator.SetBool("IsReload", false);

            Debug.Log("Reloading...");

            _state = State.射撃可能;
            _currentAmmo.Value = _parameter.magazineSize;
        }

        public Vector3 GetShotDirectionWithinSpread(Transform shootTransform)
        {
            float spreadAngleRatio = _settings.BulletSpreadAngle / 180f;
            Vector3 spreadWorldDirection = Vector3.Slerp(shootTransform.forward, UnityEngine.Random.insideUnitSphere,
                spreadAngleRatio);

            return spreadWorldDirection;
        }

        public void ShowWeapon()
        {
            ShowWeaponAsync(default).Forget();
        }

        private async UniTaskVoid ShowWeaponAsync(CancellationToken token)
        {
            _state = State.射撃不可;
            await _importSub.FirstOrDefault().ToUniTask(cancellationToken: token);
            _state = State.射撃可能;

            var brain = _cam.GetComponent<CinemachineBrain>();

            // 現在アクティブなCinemachineVirtualCameraを取得
            var virtualCamera = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
            if (virtualCamera != null)
            {
                virtualCamera.Follow = _settings.FollowTarget;
                virtualCamera.LookAt = _settings.LookTarget;
            }
        }

        public async UniTask RemoveWeapon(CancellationToken token)
        {
            _animator.SetTrigger("Remove");

            var brain = Camera.main.GetComponent<CinemachineBrain>();

            // 現在アクティブなCinemachineVirtualCameraを取得
            var virtualCamera = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
            if (virtualCamera != null)
            {
                virtualCamera.Follow = null;
                virtualCamera.LookAt = null;
            }

            await _removeSub.FirstOrDefault().ToUniTask(cancellationToken: token);
        }

        public void CompleteImport()
        {
            _importSub.OnNext(Unit.Default);
        }

        public void CompleteRemove()
        {
            _removeSub.OnNext(Unit.Default);
        }

    }

}