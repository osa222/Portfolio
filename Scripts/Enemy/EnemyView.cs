using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Battle.Game;

namespace Battle
{


    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _damageText;
        [SerializeField] private TextMeshProUGUI _weaknessText;

        private CanvasGroup _damageTextCanvasGroup;
        private CanvasGroup _weaknessCanvasGroup;

        private Enemy _enemy;
        private Vector3 _defaultPos;
        private Sequence _currentSequence;

        private Camera _cam;

        private Vector3 _baseScale;

        [SerializeField] private float minDamagePopupSize = 5;

        private void Start()
        {
            _cam = Camera.main;
            _baseScale = this.transform.localScale / minDamagePopupSize;

            _damageTextCanvasGroup = _damageText.GetComponent<CanvasGroup>();
            _weaknessCanvasGroup = _weaknessText.GetComponent<CanvasGroup>();

            _enemy = GetComponentInParent<Enemy>();

            _defaultPos = _damageText.rectTransform.localPosition;

            _enemy.OnDamage.AddListener(OnDamage);

        }

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
        }

        public void OnDamage(Damage damage)
        {


            if (_currentSequence != null)
                _currentSequence.Kill(true);

            if (Vector3.Distance(_cam.transform.position, _enemy.transform.position) >= minDamagePopupSize)
            {
                this.transform.localScale = _baseScale * this.GetDistance();
            }

            CanvasGroup damageTextCanvasGroup = damage.IsWeakness ? _weaknessCanvasGroup : _damageTextCanvasGroup;
            var damageText = damage.IsWeakness ? _weaknessText : _damageText;

            damageTextCanvasGroup.DOFade(0, 0);
            damageText.text = damage.Value.ToString();

            // ìoèÍ
            var sequence = DOTween.Sequence();
            sequence.Append(damageTextCanvasGroup.DOFade(1, 0));

            // ëﬁèÍ
            sequence.Append(damageTextCanvasGroup.DOFade(0, 1f))
                .Join(damageText.rectTransform.DOAnchorPos(new Vector2(0, 2), 1f));

            sequence.Play()
                .OnComplete(() =>
                {
                    transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
                    damageText.rectTransform.localPosition = _defaultPos;
                });


            _currentSequence = sequence;
        }

        // ÉJÉÅÉâÇ©ÇÁÇÃãóó£ÇéÊìæ
        float GetDistance()
        {
            return (this.transform.position - _cam.transform.position).magnitude;
        }

    }

}