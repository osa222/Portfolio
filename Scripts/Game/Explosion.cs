using System.Collections;
using UnityEngine;
using Battle.Game;

namespace Battle
{

    public class Explosion : MonoBehaviour
    {
        [Header("爆風に当たったときに吹っ飛ぶ力の強さ")]
        [SerializeField]
        private float _futtobiPower;

        [Header("爆風の判定が実際に発生するまでのディレイ")]
        [SerializeField]
        private float _startDelaySeconds = 0.1f;

        [Header("爆風の持続フレーム数")] [SerializeField] private int _durationFrameCount = 1;

        [Header("エフェクト含めすべての再生が終了するまでの時間")]
        [SerializeField]
        private float _stopSeconds = 2f;

        [SerializeField] private GameObject _effect;

        [SerializeField] private AudioSource _sfx;

        [Tooltip("発射物が何かに当たったときのダメージ範囲")]
        [SerializeField] private float _areaOfEffectDistance = 5f;

        [SerializeField] private LayerMask _layerMask;

        private void Awake()
        {
            _effect.SetActive(false);
            _sfx?.Stop();

        }

        /// <summary>
        /// 爆破する
        /// </summary>
        public void Explode()
        {
            // 当たり判定管理のコルーチン
            StartCoroutine(ExplodeCoroutine());
            // 爆発エフェクト含めてもろもろを消すコルーチン
            StartCoroutine(StopCoroutine());

            // エフェクトと効果音再生
            _effect.SetActive(true);
            _sfx?.Play();
        }

        private IEnumerator ExplodeCoroutine()
        {
            // 指定秒数が経過するまでFixedUpdate上で待つ
            var delayCount = Mathf.Max(0, _startDelaySeconds);
            while (delayCount > 0)
            {
                yield return new WaitForFixedUpdate();
                delayCount -= Time.fixedDeltaTime;
            }
            FindEnemies();
            // 一定フレーム数有効化
            for (var i = 0; i < _durationFrameCount; i++)
            {
                yield return new WaitForFixedUpdate();
            }

        }

        private IEnumerator StopCoroutine()
        {
            // 時間経過後に消す
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

