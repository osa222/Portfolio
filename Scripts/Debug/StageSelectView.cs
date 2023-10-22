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
            //�{�^���ȊO���N���b�N�����ꍇ�ɂ��J�[�\�����c���@syuntoku
            BattleManager.CursorEnable();

            _stage1Button.OnClickAsObservable().Subscribe(_ => Transition(_stage1Button.name)).AddTo(this);
            _stage2Button.OnClickAsObservable().Subscribe(_ => Transition(_stage2Button.name)).AddTo(this);
        }

        private void Transition(string nextStage)
        {
            //�V�[���Ɉړ�����ۂɋ����I�ɃJ�[�\���������@syuntoku
            BattleManager.CursorDisable();

            TransitionManager.LoadScene(nextStage, () => { });
        }

    }

}