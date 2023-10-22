using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;

namespace Battle.UI
{

    public class StageSelectView : MonoBehaviour
    {
        [SerializeField] private Button _stage1Button;
        [SerializeField] private Button _stage2Button;

        private void Start()
        {
            //ボタン以外をクリックした場合にもカーソルを残す　syuntoku
            BattleManager.CursorEnable();

            _stage1Button.OnClickAsObservable().Subscribe(_ => Transition(_stage1Button.name)).AddTo(this);
            _stage2Button.OnClickAsObservable().Subscribe(_ => Transition(_stage2Button.name)).AddTo(this);
        }

        private void Transition(string nextStage)
        {
            //シーンに移動する際に強制的にカーソルを消す　syuntoku
            BattleManager.CursorDisable();

            TransitionManager.LoadScene(nextStage, () => { });
        }

    }

}