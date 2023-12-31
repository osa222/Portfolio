﻿using UnityEngine;
using System;

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

    [Serializable]
    public class MachineGunStatus
    {
        public float objectDistance;
    }

    [Serializable]
    public class ShotgunStatus
    {
        public float radius = 1f;
        public float 射程 = 1.0f;
        public int 散弾数 = 150;
        public int damage = 10;
    }

    [Serializable]
    public class LauncherStatus
    {
        public GameObject launcherProjectilePrefab;
        public float radius = 1f;
        public float 射程 = 1.0f;
        public int damage = 10;
    }
}