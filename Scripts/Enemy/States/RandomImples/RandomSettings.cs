using System;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Battle
{

    [CreateAssetMenu]
    public class RandomSettings : ScriptableObject
    {
        public float _defaultMoveSpeed = 1f;
        public float _minRandomSpeed = 0.8f, _maxRandomSpeed = 1.2f;

        public float rotationSpeed = 1f;
        [NonSerialized] public float speed = 1f;

        private void OnEnable()
        {
            speed = _defaultMoveSpeed * Random.Range(_minRandomSpeed, _maxRandomSpeed);
        }
    }
}