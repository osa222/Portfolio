using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

namespace Battle
{
    public class PlayerHUD : MonoBehaviour
    {

        [SerializeField] private Image _hpbar;
        [SerializeField] private TextMeshProUGUI _hpText;

        [SerializeField] private Image _hurtImage;

        [SerializeField] private Image _reticleImage;
        [SerializeField] private TextMeshProUGUI _AmmoText;

        private Player _player;
        private PlayerWeaponController _playerWeapon;

        private void Start()
        {
            TryGetComponent(out _player);
            TryGetComponent(out _playerWeapon);

            _player.CurrentHP.Subscribe(ShowHP).AddTo(this);

            _player.CurrentHealthState.Subscribe(_ =>
            {
                // 瀕死状態なら、瀕死画像を表示
                _hurtImage.enabled = _player.CurrentHealthState.Value == Player.HealthState.Dying ? true : false;

            }).AddTo(this);

            _playerWeapon.OnWeaponChanged.Subscribe(_ =>
            {
                _reticleImage.sprite = _playerWeapon.CurrentWeapon.Weapon.weaponData.reticleImage;
                _reticleImage.SetNativeSize();

                var currentWeapon = _playerWeapon.CurrentWeapon;

                _AmmoText.text = $"{currentWeapon.CurrentAmmo.Value} / {currentWeapon._parameter.magazineSize}";

                // 武器チェン毎に購読するため、バグってる可能性あり
                currentWeapon.CurrentAmmo.Subscribe(x => _AmmoText.text = $"{currentWeapon.CurrentAmmo.Value} / {currentWeapon._parameter.magazineSize}").AddTo(this);

            }).AddTo(this);

            _player.OnDamage.Subscribe(x => Register(x.DamageSource.transform)).AddTo(this);
        }

        private void Register(Transform damageSource)
        {
            if (!DI_System.CheckIfObjectInSight(damageSource))
            {
                DI_System.CreateIndicator(damageSource);
            }
        }

        void ShowHP(int hp)
        {
            _hpText.text = $"HP {hp}/{_player.MaxHP}";
            _hpbar.fillAmount = (float)_player.CurrentHP.Value / _player.MaxHP;
        }

    }


}
