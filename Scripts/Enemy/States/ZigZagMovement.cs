using UnityEngine;

namespace Battle
{

    public class ZigZagMovement : MonoBehaviour
    {
        // ジグザグ移動に使うパラメータ
        [SerializeField] private float _pingPong = 1f;
        [Header("Playerの方向を向く度合い")]
        [SerializeField] private int _angle = 30;
        private GameObject _player;
        private Vector3 _previousPos;

        [SerializeField] private float _moveSpeed = 1f;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _previousPos = transform.position;
        }

        private void Update()
        {
            ZigZagMove();
        }

        public void ZigZagMove()
        {
            float move = 0f;

            if (transform.localPosition.x <= _pingPong)
            {
                move = Mathf.PingPong(Time.time * _moveSpeed, _pingPong);
            }
            else if (transform.localPosition.x >= _pingPong)
            {
                move = -Mathf.PingPong(Time.time * _moveSpeed, _pingPong);
            }

            var pos = transform.localPosition;
            pos.x = move;
            transform.localPosition = pos;

            var dir = (transform.position - _previousPos).normalized;
            transform.rotation = Quaternion.LookRotation(dir);

            Vector3 forwardDirection = transform.forward;
            Vector3 targetDirection = _player.transform.position - transform.position;

            Vector3 newDirection = Vector3.RotateTowards(forwardDirection, targetDirection, Mathf.Deg2Rad * _angle, 0f);
            transform.rotation = Quaternion.LookRotation(newDirection);

            _previousPos = transform.position;
        }
    }

}