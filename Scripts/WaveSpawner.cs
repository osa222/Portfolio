using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Battle
{

    public class WaveSpawner : MonoBehaviour
    {
        [SerializeField] private Wave[] _waves;
        [SerializeField] private int _radius;
        [SerializeField] private Transform _rootObject;

        private int _nextWave = 0;

        public int _enemiesAlive;

        private IEnumerator _spawnCoroutine;
        public void StartSpawner()
        {
            var currentWave = _waves[_nextWave];
            if (currentWave.spawnIntarval == 0)
                GenerateEnemy();
            else
            {
                _spawnCoroutine = GenerateEnemyCoroutine();
                StartCoroutine(_spawnCoroutine);
            }
        }

        public void StopSpawner()
        {
            StopCoroutine(_spawnCoroutine);
        }

        private IEnumerator GenerateEnemyCoroutine()
        {
            var currentWave = _waves[_nextWave];

            _enemiesAlive = currentWave.count;

            for (var i = 0; i < currentWave.count; i++)
            {
                var prefab = currentWave.spawnEnemies[Random.Range(0, currentWave.spawnEnemies.Length)];

                var pos = GetPosition(prefab);
                var enemy = Instantiate(prefab, pos, Quaternion.identity);

                if (_rootObject != null)
                    enemy.transform.SetParent(_rootObject);

                yield return new WaitForSeconds(currentWave.spawnIntarval);
            }
        }

        private void GenerateEnemy()
        {
            var currentWave = _waves[_nextWave];

            _enemiesAlive = currentWave.count;

            for (var i = 0; i < currentWave.count; i++)
            {
                var prefab = currentWave.spawnEnemies[Random.Range(0, currentWave.spawnEnemies.Length)];

                var pos = GetPosition(prefab);
                pos += transform.position;
                var enemy = Instantiate(prefab, pos, Quaternion.identity);

                if (_rootObject != null)
                    enemy.transform.SetParent(_rootObject);
            }
        }

        [Header("��s����G�̃����_���ȍ����͈̔�")]
        [SerializeField] private float _�ŏ����� = 1f, _�ő卂�� = 2f;
        private Vector3 GetPosition(Enemy spawnEnemy)
        {
            // �~����̃����_���ȍ��W0,360�̃����_���o���Ĕ��a���l�������Ƃ��̈ʒu���v�Z
            var randomdeg = Random.Range(0, Mathf.PI * 2);
            var pos = new Vector3(Mathf.Cos(randomdeg) * _radius, 0f, Mathf.Sin(randomdeg) * _radius);

            pos += transform.position;
            pos.y = 0;

            // ���s�G�͒n�ʂɐڒn����悤�n�ʂ̈ʒu���v�Z
            if (spawnEnemy.EnemyParameter.Type != EnemyType.Fly)
            {
                var rayPos = pos + new Vector3(0, 100, 0);
                if (Physics.Raycast(rayPos, Vector3.down, out var hit, Mathf.Infinity))
                {
                    pos.y = hit.point.y;
                }

                return pos;
            }

            // ��s�G�̏ꍇ�A�����������_���ɂ���
            var y = Random.Range(_�ŏ�����, _�ő卂��);
            pos.y = y;
            return pos;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }

}