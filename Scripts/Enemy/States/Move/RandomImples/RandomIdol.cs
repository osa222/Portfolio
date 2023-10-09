using UnityEngine;

namespace Battle.Enemies
{

    public class RandomIdol : RandomMove
    {
        public RandomSettings _settings;
        private Animator _animator;
        private GameObject _player;

        public override void Init()
        {
            _animator = GetComponent<Animator>();
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        public override void OnStateEnter()
        {
            _animator.Play("Idol");
        }

        public override void OnStateUpdate()
        {
            var dir = (_player.transform.position - transform.position).normalized;

            var rotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _settings.rotationSpeed * Time.deltaTime);
        }

        public override void OnStateExit()
        {

        }
    }




}