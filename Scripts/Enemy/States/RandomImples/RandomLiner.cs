using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Battle
{

    public class RandomLiner : RandomMove
    {
        public RandomSettings _settings;
        private GameObject _player;

        public override void Init()
        {

        }

        public override void OnStateEnter()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        public override void OnStateUpdate()
        {

            var dir = (_player.transform.position - transform.position).normalized;
            transform.position += dir * _settings.speed * Time.deltaTime;

            transform.LookAt(_player.transform);
        }

        public override void OnStateExit()
        {

        }
    }
}