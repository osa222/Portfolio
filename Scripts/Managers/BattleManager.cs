using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Battle
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public int KillCount
        {
            get => _killRP.Value;
            set => _killRP.Value = value;
        }

        public IReadOnlyReactiveProperty<int> CurrentKillCount => _killRP;
        private ReactiveProperty<int> _killRP = new ReactiveProperty<int>(0);

        public IReadOnlyReactiveProperty<int> WaveCount => _waveCountRP;
        private ReactiveProperty<int> _waveCountRP = new ReactiveProperty<int>(0);

        public IReadOnlyReactiveProperty<int> Remainingtime => _timer.Remainingtime;
        private Timer _timer = new Timer(0);
        [SerializeField] private int _remainingtime;

        [SerializeField] private WaveSpawner[] _waveSpawners;

        private void Start()
        {
            _timer.Initialize(_remainingtime);
            _killRP.AddTo(this);
            _waveCountRP.AddTo(this);

            LoopAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid LoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Debug.Log("BattleStart...");
                await BattleStart(token);

                Debug.Log("BattleFinish...");
                await UniTask.Delay(5000, cancellationToken: token);
            }
        }

        public async UniTask BattleStart(CancellationToken token)
        {
            _waveCountRP.Value++;
            _timer.Initialize(_remainingtime);

            foreach (var spawner in _waveSpawners) spawner.StartSpawner();

            await _timer.CountStartAsync(token);

            foreach (var spawner in _waveSpawners) spawner.StopSpawner();

        }


        public void SpawnEnemy(int spawnCount)
        {
            var count = spawnCount / _waveSpawners.Length;
            foreach (var spawner in _waveSpawners) spawner.SpawnEnemy(count);
        }

        public void UIMode(bool isUiMode)
        {
            if (isUiMode)
            {
                CursorEnable();
            }
            else
            {
                CursorDisable();
            }
        }

        public static void CursorEnable()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public static void CursorDisable()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDestroy()
        {
            _timer.Dispose();
        }
    }

}