using UnityEngine;

namespace Battle
{

    [CreateAssetMenu]
    public class WeaponData : ScriptableObject
    {
        public string weaponName;
        public int damage;

        public int magazineSize;

        [Tooltip("発射間隔")]
        public float shootInterval;

        [Tooltip("銃口回転速度")]
        public int muzzleRotationSpeed;

        [Tooltip("攻撃範囲")]
        public float particleScale;

        public float reloadTime;
        public Sprite reticleImage;
    }
}