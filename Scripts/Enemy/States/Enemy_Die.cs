using System.Collections;
using UnityEngine;

namespace Battle.Enemies
{

    internal class Enemy_Die : MonoBehaviour, IEnemyState
    {

        [Header("死亡時の落下速度、破壊までの時間")]
        [SerializeField] private float _fallSpeedOnDeath = 1.0f;
        [SerializeField] private float _fallTimeOnDeath = 10f;

        [SerializeField] private GameObject _vfx;

        private Animator _animator;
        private static readonly int HashDie = Animator.StringToHash("Die");

        public void Init()
        {
            TryGetComponent(out _animator);
        }

        public void OnStateEnter()
        {
            StartCoroutine(Die());
        }

        public void OnStateExit()
        {

        }

        private IEnumerator Die()
        {
            _animator.SetTrigger(HashDie);

            // var effectObj = Instantiate(_vfx, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            //Destroy(effectObj.gameObject, effectObj.main.duration);

            Destroy(this.gameObject, 2f);

            if (BattleManager.Instance != null)
            {
                BattleManager.Instance.KillCount++;
            }

            var timer = 0f;
            while (timer <= _fallTimeOnDeath)
            {
                timer += Time.deltaTime;
                transform.position += Vector3.down * _fallSpeedOnDeath * Time.deltaTime;
                yield return null;
            }

            // SE,Effect再生
        }
        public void OnUpdate()
        {

        }
    }

}