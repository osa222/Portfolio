using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// DamageindicatorƒVƒXƒeƒ€
    /// </summary>
    public class DI_System : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private DamageIndicator damageIndicatorPrefab;
        [SerializeField] private RectTransform holder;
        [SerializeField] private new Camera camera;
        [SerializeField] private Transform player;

        private Dictionary<Transform, DamageIndicator> Indicators = new Dictionary<Transform, DamageIndicator>();

        #region Delegates
        public static Action<Transform> CreateIndicator = delegate { };
        public static Func<Transform, bool> CheckIfObjectInSight;
        #endregion

        private void OnEnable()
        {
            CreateIndicator += Create;
            CheckIfObjectInSight += InSight;
        }

        private void OnDisable()
        {
            CreateIndicator -= Create;
            CheckIfObjectInSight -= InSight;
        }

        private void Create(Transform target)
        {
            if (Indicators.ContainsKey(target))
            {
                Indicators[target].ReStart();
                return;
            }

            var newIndicator = Instantiate(damageIndicatorPrefab, holder);
            newIndicator.Register(target, player, new Action(() => { Indicators.Remove(target); }));

            Indicators.Add(target, newIndicator);
        }

        private bool InSight(Transform t)
        {
            var screenPoint = camera.WorldToViewportPoint(t.position);
            return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        }
    }

}