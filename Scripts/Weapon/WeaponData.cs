using UnityEngine;

namespace Battle
{

    [CreateAssetMenu]
    public class WeaponData : ScriptableObject
    {
        public int damage;
        public int magazineSize;
        public float shootInterval;
        public int 銃口回転速度;
        public float 攻撃範囲;
        public float ReloadTime;
        public Sprite reticleImage;
    }
}