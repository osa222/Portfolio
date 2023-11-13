using UnityEngine;
using Battle.Game;

namespace Battle
{

    public abstract class BaseBullet : MonoBehaviour
    {
        public GameObject Owner { get; private set; }
        public Vector3 InitialPosition { get; private set; }
        public Vector3 InitialDirection { get; private set; }
        public Damage Damage { get; private set; }
        public void Shoot(Damage damage)
        {
            Damage = damage;
            Owner = damage.DamageSource;
            InitialPosition = transform.position;
            InitialDirection = transform.forward;

            OnShoot();
        }

        public abstract void OnShoot();
    }

}