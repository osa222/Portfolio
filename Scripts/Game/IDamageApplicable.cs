using UnityEngine;

namespace Battle.Game
{

    public interface IDamageApplicable
    {
        public void ApplyDamage(Damage damage);
    }

    // ボックス化？
    public struct Damage
    {
        public int Value { get; }
        public GameObject DamageSource { get; }
        public bool IsExplosionDamage { get; }
        public bool IsWeakness { get; }

        public Damage(int value, GameObject damageSource, bool isExplosion, bool isWeakness = false)
        {
            Value = value;
            DamageSource = damageSource;
            IsExplosionDamage = isExplosion;
            IsWeakness = isWeakness;
        }
    }
}