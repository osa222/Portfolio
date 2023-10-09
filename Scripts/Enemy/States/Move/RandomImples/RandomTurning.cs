using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Battle.Enemies
{

    public class RandomTurning : RandomMove
    {

        public RandomSettings _settings;
        private GameObject _player;
        private Vector3 _previousPos;
        private float _dir;

        [SerializeField] private int _angle;
        [SerializeField] private bool _isLookPlayer = false;

        public override void Init()
        {

        }

        public override void OnStateEnter()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _dir = GetDirection();
        }

        private float GetDirection()
        {
            bool random = Random.Range(0, 2) == 0;
            return random ? 1 : -1;
        }

        public override void OnStateUpdate()
        {
            transform.RotateAround(_player.transform.position, Vector3.up, _dir * _settings.speed * 2 * Time.deltaTime);

            var dir = (transform.position - _previousPos).normalized;
            var rotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _settings.rotationSpeed * Time.deltaTime);

            _previousPos = transform.position;

            if (_isLookPlayer)
                LookAtPlayer();
        }

        private void LookAtPlayer()
        {

            //現在のオブジェクトの進行方向を取得
            Vector3 forwardDirection = transform.forward;

            // ターゲットオブジェクトへのベクトルを取得
            Vector3 targetDirection = _player.transform.position - transform.position;

            // 30度だけ回転させる
            Vector3 newDirection = Vector3.RotateTowards(forwardDirection, targetDirection, Mathf.Deg2Rad * _angle * Time.deltaTime, 0f);

            // 回転後の方向に向かせる（滑らかに回転させる場合、Time.deltaTime を乗算することがあります）
            transform.rotation = Quaternion.LookRotation(newDirection);

            _previousPos = transform.position;
        }

        public override void OnStateExit()
        {

        }
    }
}