using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace DebugMode.Battle
{

    public class DebugPresenter : MonoBehaviour
    {
        [SerializeField] private Toggle _weaponTggle;
        [SerializeField] private Toggle _battleTggle;
        [SerializeField] private Toggle _effectTggle;

        [SerializeField] private Canvas _weaponScreen;
        [SerializeField] private Canvas _battleScreen;
        [SerializeField] private Canvas _effectScreen;

        [SerializeField] private Button _spawnApplyButton;
        [SerializeField] private InputField _spawnCountIF;

        [SerializeField] private EffectScalerView _fireView;
        [SerializeField] private EffectScalerView _laserView;

        [SerializeField] private DebugManager _debugManager;

        private void Start()
        {
            Init();

            _weaponTggle.OnValueChangedAsObservable().Subscribe(x =>
            {
                _weaponScreen.enabled = x ? true : false;
            }).AddTo(this);

            _battleTggle.OnValueChangedAsObservable().Subscribe(x =>
            {
                _battleScreen.enabled = x ? true : false;
            }).AddTo(this);

            _effectTggle.OnValueChangedAsObservable().Subscribe(x =>
            {
                if (_effectScreen) _effectScreen.enabled = x ? true : false;
            }).AddTo(this);

            _spawnApplyButton
                .OnClickAsObservable()
                .Select(_ => int.Parse(_spawnCountIF.text))
                .Subscribe(x =>
                {
                    _debugManager.SpawnEnemy(x);
                    _spawnCountIF.text = "";
                })
                .AddTo(this);

            // EffectView
            _fireView.SetScale(_debugManager.fireEffect.localScale);
            _laserView.SetScale(_debugManager.laserEffect.localScale);

            _fireView.CurrentScale.Subscribe(x => _debugManager.fireEffect.localScale = x).AddTo(this);
            _laserView.CurrentScale.Subscribe(x => _debugManager.laserEffect.localScale = x).AddTo(this);
        }

        private void Init()
        {
            _weaponScreen.enabled = false;
            _battleScreen.enabled = false;
            _effectScreen.enabled = false;
        }


    }

}