using System.Collections;
using UnityEngine;
using System;

namespace Battle
{

    public class DamageIndicator : MonoBehaviour
    {
        //public RectTransform indicator;

        //public Transform weapon;

        private const float MaxTimer = 0.8f;
        private float timer = MaxTimer;

        private CanvasGroup canvasGroup;
        protected CanvasGroup CanvasGroup
        {
            get
            {
                if (canvasGroup == null)
                {
                    if (!TryGetComponent(out canvasGroup))
                    {
                        canvasGroup = gameObject.AddComponent<CanvasGroup>();
                    }
                }
                return canvasGroup;
            }
        }

        private RectTransform rect;
        protected RectTransform Rect
        {
            get
            {
                if (rect == null)
                {
                    if (!TryGetComponent(out rect))
                    {
                        rect = gameObject.AddComponent<RectTransform>();
                    }
                }
                return rect;
            }
        }

        public Transform Target { get; protected set; } = null;
        private Transform player;

        private IEnumerator IE_CountDown;
        private Action unRegister;

        private Quaternion tRot = Quaternion.identity;
        private Vector3 tPos = Vector3.zero;

        public void Register(Transform target, Transform player, Action unRegister)
        {
            this.Target = target;
            this.player = player;
            this.unRegister = unRegister;

            StartCoroutine(RotateToTheTarget());
            StartTimer();
        }

        public void ReStart()
        {
            timer = MaxTimer;
            StartTimer();
        }

        private void StartTimer()
        {
            if (IE_CountDown != null) { StopCoroutine(IE_CountDown); }
            IE_CountDown = CountDown();
            StartCoroutine(IE_CountDown);
        }

        private IEnumerator CountDown()
        {
            Debug.Log("CountDown");
            while (CanvasGroup.alpha < 1.0f)
            {
                CanvasGroup.alpha += 4 * Time.deltaTime;
                yield return null;
            }
            while (timer > 0)
            {
                timer--;
                yield return new WaitForSeconds(1f);
            }
            while (CanvasGroup.alpha > 0.0f)
            {
                CanvasGroup.alpha -= 2 * Time.deltaTime;
                yield return null;
            }
            unRegister();
            Destroy(this.gameObject);
        }

        private IEnumerator RotateToTheTarget()
        {
            while (enabled)
            {
                if (Target)
                {
                    tPos = Target.position;
                    tRot = Target.rotation;
                }
                var dir = player.position - tPos;

                tRot = Quaternion.LookRotation(dir);
                tRot.z = -tRot.y;
                tRot.x = 0;
                tRot.y = 0;


                var northDir = new Vector3(0, 0, player.eulerAngles.y);
                Rect.localRotation = tRot * Quaternion.Euler(northDir);

                yield return null;
            }
        }
    }

}