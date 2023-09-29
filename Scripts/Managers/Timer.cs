using System.Collections;
using UnityEngine;
using UniRx;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Battle
{

    public class Timer : IDisposable
    {
        public IReadOnlyReactiveProperty<int> Remainingtime => _remainingtimeRP;
        private ReactiveProperty<int> _remainingtimeRP;

        public int CountTime;

        public Timer(int countTime)
        {
            CountTime = countTime;
            _remainingtimeRP = new ReactiveProperty<int>(countTime);
        }

        public void Initialize(int countTime)
        {
            CountTime = countTime;
            _remainingtimeRP.Value = countTime;
        }


        public async UniTask CountStartAsync(CancellationToken token)
        {
            while (_remainingtimeRP.Value > 0 && !token.IsCancellationRequested)
            {
                //token.ThrowIfCancellationRequested();
                await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
                _remainingtimeRP.Value--;
            }
        }

        public void Dispose()
        {
            _remainingtimeRP.Dispose();
        }
    }

}