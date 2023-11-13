using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Battle.Weapons
{

    public class Shotgun : BaseWeapon
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private float _range = 1.0f;

        [Header("散弾数")]
        [SerializeField] private int _pelletCount = 150;
        public int _damage = 10;

        [SerializeField] private bool _isDrawGizmo = true;

        private List<Vector3> _layPoslist = new List<Vector3>();

        protected override void Fire()
        {
            _layPoslist.Clear();

            var damageEnemies = new List<DamageEnemy>();

            // 射程上のランダムな座標を取得する
            for (int i = 0; i < _pelletCount; i++)
            {
                Vector3 adjustedPosition = CalculateHitPosition(_cam.transform);


                // レイキャストを実行
                Vector3 rayStart = _cam.transform.position;
                Vector3 rayDirection = adjustedPosition - _cam.transform.position;
                RaycastHit hit;
                Vector3 RayPos = adjustedPosition;
                if (Physics.Raycast(rayStart, rayDirection, out hit, _range))
                {

                    var enemy = hit.collider.GetComponentInParent<Enemy>();
                    if (enemy)
                    {
                        // 散弾が命中した敵ごとに与えるダメージを計算する
                        var damageEnemy = damageEnemies.FirstOrDefault(x => x.enemy == enemy);

                        if (damageEnemy == null)
                            damageEnemies.Add(new DamageEnemy(enemy, _damage));
                        else
                            damageEnemy.AddDamage(_damage);
                    }
                    RayPos = hit.point;
                }
                else
                {
                    // レイキャストがヒットしなかった場合の処理

                }

                _layPoslist.Add(RayPos);
            }

            // 散弾が命中した敵にダメージを当たる
            foreach (var damageEnemy in damageEnemies)
            {
                damageEnemy.enemy.TakeDamage(new Game.Damage(damageEnemy.damage, this.gameObject, false));
            }

        }

        /// <summary>
        ///  散弾ごとに命中する座標を計算
        /// </summary>
        private Vector3 CalculateHitPosition(Transform shootTransform)
        {

            Vector3 randomPosition = Random.insideUnitCircle * _radius;
            Vector3 range = shootTransform.position + shootTransform.forward * _range; // 自分の前方向に射程分離れた座標を計算

            // ローカル座標系からワールド座標系へ変換し、Y軸回転に合わせて位置を調整
            Vector3 adjustedPosition = range + shootTransform.rotation * randomPosition;
            return adjustedPosition;
        }

        private void OnDrawGizmosSelected()
        {
            if (_isDrawGizmo)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_cam.transform.position + _cam.transform.forward * _range, _radius);

                foreach (var pos in _layPoslist)
                {
                    Gizmos.DrawLine(_cam.transform.position, pos);
                }
            }
        }

        class DamageEnemy
        {
            public Enemy enemy;
            public int damage;

            public DamageEnemy(Enemy enemy, int damage)
            {
                this.enemy = enemy;
                this.damage = damage;
            }

            public void AddDamage(int damage)
            {
                this.damage += damage;
            }
        }
    }

}