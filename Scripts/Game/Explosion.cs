using System.Collections;
using UnityEngine;
using Battle.Game;

namespace Battle
{

    public class Explosion : MonoBehaviour
    {
        [Header("�����ɓ��������Ƃ��ɐ�����ԗ͂̋���")]
        [SerializeField]
        private float _futtobiPower;

        [Header("�����̔��肪���ۂɔ�������܂ł̃f�B���C")]
        [SerializeField]
        private float _startDelaySeconds = 0.1f;

        [Header("�����̎����t���[����")] [SerializeField] private int _durationFrameCount = 1;

        [Header("�G�t�F�N�g�܂߂��ׂĂ̍Đ����I������܂ł̎���")]
        [SerializeField]
        private float _stopSeconds = 2f;

        [SerializeField] private GameObject _effect;

        [SerializeField] private AudioSource _sfx;

        [Tooltip("���˕��������ɓ��������Ƃ��̃_���[�W�͈�")]
        [SerializeField] private float _areaOfEffectDistance = 5f;

        [SerializeField] private LayerMask _layerMask;

        private void Awake()
        {
            _effect.SetActive(false);
            _sfx?.Stop();

        }

        /// <summary>
        /// ���j����
        /// </summary>
        public void Explode()
        {
            // �����蔻��Ǘ��̃R���[�`��
            StartCoroutine(ExplodeCoroutine());
            // �����G�t�F�N�g�܂߂Ă������������R���[�`��
            StartCoroutine(StopCoroutine());

            // �G�t�F�N�g�ƌ��ʉ��Đ�
            _effect.SetActive(true);
            _sfx?.Play();
        }

        private IEnumerator ExplodeCoroutine()
        {
            // �w��b�����o�߂���܂�FixedUpdate��ő҂�
            var delayCount = Mathf.Max(0, _startDelaySeconds);
            while (delayCount > 0)
            {
                yield return new WaitForFixedUpdate();
                delayCount -= Time.fixedDeltaTime;
            }
            FindEnemies();
            // ���t���[�����L����
            for (var i = 0; i < _durationFrameCount; i++)
            {
                yield return new WaitForFixedUpdate();
            }

        }

        private IEnumerator StopCoroutine()
        {
            // ���Ԍo�ߌ�ɏ���
            yield return new WaitForSeconds(_stopSeconds);
            _effect.SetActive(false);
            _sfx?.Stop();

            Destroy(gameObject);
        }

        private void FindEnemies()
        {
            var affectedColliders = Physics.OverlapSphere(transform.position, _areaOfEffectDistance, _layerMask, QueryTriggerInteraction.Collide);
            foreach (var col in affectedColliders)
            {
                if (col.TryGetComponent<IDamageApplicable>(out var damageApplicable))
                {
                    Debug.Log("aaa");
                    damageApplicable.ApplyDamage(new Damage(150, this.gameObject, true));
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red * 0.5f;
            Gizmos.DrawSphere(transform.position, _areaOfEffectDistance);
        }
    }
}

