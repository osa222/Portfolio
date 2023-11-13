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

        [Header("�U�e��")]
        [SerializeField] private int _pelletCount = 150;
        public int _damage = 10;

        [SerializeField] private bool _isDrawGizmo = true;

        private List<Vector3> _layPoslist = new List<Vector3>();

        protected override void Fire()
        {
            _layPoslist.Clear();

            var damageEnemies = new List<DamageEnemy>();

            // �˒���̃����_���ȍ��W���擾����
            for (int i = 0; i < _pelletCount; i++)
            {
                Vector3 adjustedPosition = CalculateHitPosition(_cam.transform);


                // ���C�L���X�g�����s
                Vector3 rayStart = _cam.transform.position;
                Vector3 rayDirection = adjustedPosition - _cam.transform.position;
                RaycastHit hit;
                Vector3 RayPos = adjustedPosition;
                if (Physics.Raycast(rayStart, rayDirection, out hit, _range))
                {

                    var enemy = hit.collider.GetComponentInParent<Enemy>();
                    if (enemy)
                    {
                        // �U�e�����������G���Ƃɗ^����_���[�W���v�Z����
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
                    // ���C�L���X�g���q�b�g���Ȃ������ꍇ�̏���

                }

                _layPoslist.Add(RayPos);
            }

            // �U�e�����������G�Ƀ_���[�W�𓖂���
            foreach (var damageEnemy in damageEnemies)
            {
                damageEnemy.enemy.TakeDamage(new Game.Damage(damageEnemy.damage, this.gameObject, false));
            }

        }

        /// <summary>
        ///  �U�e���Ƃɖ���������W���v�Z
        /// </summary>
        private Vector3 CalculateHitPosition(Transform shootTransform)
        {

            Vector3 randomPosition = Random.insideUnitCircle * _radius;
            Vector3 range = shootTransform.position + shootTransform.forward * _range; // �����̑O�����Ɏ˒������ꂽ���W���v�Z

            // ���[�J�����W�n���烏�[���h���W�n�֕ϊ����AY����]�ɍ��킹�Ĉʒu�𒲐�
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