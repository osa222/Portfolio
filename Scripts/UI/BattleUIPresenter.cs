using UnityEngine;
using TMPro;
using UniRx;

namespace Battle
{

    public class BattleUIPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _killCountText;
        [SerializeField] private TextMeshProUGUI _waveCountText;

        [SerializeField] private TextMeshProUGUI _waveTimer;

        [SerializeField] private BattleManager _battleSystem;

        private void Start()
        {
            _battleSystem.CurrentKillCount.Subscribe(x => _killCountText.text = x.ToString()).AddTo(this);

            _battleSystem.WaveCount.Subscribe(x => _waveCountText.text = $"Wave {x}").AddTo(this);

            _battleSystem.Remainingtime.Subscribe(x => _waveTimer.text = x.ToString()).AddTo(this);
        }


    }

}