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

            //���݂̃I�u�W�F�N�g�̐i�s�������擾
            Vector3 forwardDirection = transform.forward;

            // �^�[�Q�b�g�I�u�W�F�N�g�ւ̃x�N�g�����擾
            Vector3 targetDirection = _player.transform.position - transform.position;

            // 30�x������]������
            Vector3 newDirection = Vector3.RotateTowards(forwardDirection, targetDirection, Mathf.Deg2Rad * _angle * Time.deltaTime, 0f);

            // ��]��̕����Ɍ�������i���炩�ɉ�]������ꍇ�ATime.deltaTime ����Z���邱�Ƃ�����܂��j
            transform.rotation = Quaternion.LookRotation(newDirection);

            _previousPos = transform.position;
        }

        public override void OnStateExit()
        {

        }
    }
}