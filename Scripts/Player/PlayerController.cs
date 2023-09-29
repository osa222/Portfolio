using UnityEngine;
using UnityEngine.InputSystem;
using Battle.GamePlay;
using System;

namespace Battle
{

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Game.WeaponController _weapon;

        private PlayerInput _input;
        private WeaponRotetar _rotetar;

        private void Awake()
        {
            TryGetComponent(out _input);
            TryGetComponent(out _rotetar);
        }

        private void Update()
        {
            var inputFire = _input.actions["Fire"];
            _weapon.HandleShootInputs(inputFire.IsPressed());
        }

        private void OnEnable()
        {
            _input.actions["Look"].performed += OnLook;
        }

        private void OnDisable()
        {
            _input.actions["Look"].performed -= OnLook;
        }

        private void OnLook(InputAction.CallbackContext obj)
        {
            var stick = obj.ReadValue<Vector2>();
            _rotetar.SetDirection(stick);
        }


        private void OnFire(InputAction.CallbackContext obj)
        {
            _weapon.HandleShootInputs(true);
        }


    }

}