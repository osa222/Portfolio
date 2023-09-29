using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Game
{

    public class DamageApplicable : MonoBehaviour, IDamageApplicable
    {
        [SerializeField] private float _damageMultiplier = 1f;


        [SerializeField] private bool _IsWeakness;

        public Enemy Enemy { get; private set; }

        private void Awake()
        {
            Enemy = GetComponent<Enemy>();
            if (Enemy == null)
                Enemy = GetComponentInParent<Enemy>();
        }

        public void ApplyDamage(Damage damage)
        {
            float totalDamage = damage.Value;

            if (_IsWeakness)
                totalDamage *= _damageMultiplier;

            var newDamage = new Damage((int)totalDamage, damage.DamageSource, damage.IsExplosionDamage, _IsWeakness);
            Enemy.TakeDamage(newDamage);


        }
    }
}