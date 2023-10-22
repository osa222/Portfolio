using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Battle;
namespace DebugMode.Battle
{

    public class DebugScreen : MonoBehaviour
    {

        [SerializeField] private Text _weaponText;
        [SerializeField] private InputField _damageInput;
        [SerializeField] private InputField _speedInput;
        [SerializeField] private InputField _SpeadOfRotation;

        [SerializeField] private WeaponData _weaponData;

        private void Start()
        {
            _weaponText.text = _weaponData.weaponName;

            _damageInput.text = _weaponData.damage.ToString();
            _speedInput.text = _weaponData.shootInterval.ToString();
            _SpeadOfRotation.text = _weaponData.muzzleRotationSpeed.ToString();

            _damageInput.OnValueChangedAsObservable()
                .Select(x =>
                {
                    var isSucceed = int.TryParse(x, out var value);
                    return (isSucceed, value);
                })
                .Where(x => x.isSucceed)
                .Subscribe(x => _weaponData.damage = x.value)
                .AddTo(this);
        }
    }

}