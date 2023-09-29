using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Game
{

    public class BulletStandard : BaseBullet
    {
        [SerializeField] private float _speed = 1f;


        [Header("General")]
        [SerializeField] private float _maxLifeTime = 5f;
        [Tooltip("この発射体の衝突検出の半径")]
        public float Radius = 0.01f;
        [SerializeField] private Transform Tip;
        public LayerMask HittableLayers = -1;
        [Tooltip("発射体のルートを表す変換 (正確な衝突検出に使用)")]
        public Transform Root;


        List<Collider> m_IgnoredColliders;
        private Vector3 _LastRootPosition;
        private Vector3 _velocity;

        const QueryTriggerInteraction k_TriggerInteraction = QueryTriggerInteraction.Collide;

        private void OnEnable()
        {
            Destroy(gameObject, _maxLifeTime);
        }

        public override void OnShoot()
        {
            _velocity = InitialDirection * _speed;
        }

        private void Update()
        {
            transform.position += _velocity * Time.deltaTime;

            // 衝突検出
            {
                var closestHit = new RaycastHit();
                closestHit.distance = Mathf.Infinity;
                var foundHit = false;


                var displacementSinceLastFrame = Tip.position - _LastRootPosition;
                var hits = Physics.SphereCastAll(_LastRootPosition, Radius,
                    displacementSinceLastFrame.normalized, displacementSinceLastFrame.magnitude, HittableLayers, k_TriggerInteraction);

                foreach (var hit in hits)
                {
                    if (IsHitValid(hit) && hit.distance < closestHit.distance)
                    {
                        foundHit = true;
                        closestHit = hit;
                    }
                }

                if (foundHit)
                {
                    if (closestHit.distance <= 0f)
                    {
                        closestHit.point = Root.position;
                        closestHit.normal = -transform.forward;
                    }

                    OnHit(closestHit.point, closestHit.normal, closestHit.collider);
                }

            }

            _LastRootPosition = Root.position;
        }

        private void OnHit(Vector3 point, Vector3 normal, Collider collider)
        {
            var damageable = collider.GetComponent<IDamageApplicable>();
            if (damageable == null) return;

            damageable.ApplyDamage(Damage);

            Destroy(this.gameObject);
        }

        bool IsHitValid(RaycastHit hit)
        {
            // ignore hits with an ignore component
            if (hit.collider.GetComponent<IgnoreHitDetection>())
            {
                return false;
            }

            // ignore hits with triggers that don't have a Damageable component
            if (hit.collider.isTrigger && hit.collider.GetComponent<IDamageApplicable>() == null)
            {
                return false;
            }

            // ignore hits with specific ignored colliders (self colliders, by default)
            if (m_IgnoredColliders != null && m_IgnoredColliders.Contains(hit.collider))
            {
                return false;
            }

            return true;
        }
    }
}