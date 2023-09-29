using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace DebugMode.Battle
{

    public class BattleParameterDebugView : MonoBehaviour
    {
        [SerializeField] private Toggle _debugModeTggle;
        [SerializeField] private Canvas _debugScreen;

        private void Start()
        {
            _debugModeTggle.OnValueChangedAsObservable().Subscribe(x =>
            {
                _debugScreen.enabled = x ? true : false;
            }).AddTo(this);
        }
    }

}