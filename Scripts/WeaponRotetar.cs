using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle.Game;

namespace Battle.GamePlay
{

    public class WeaponRotetar : MonoBehaviour
    {
        private PlayerWeaponController _playerWeaponController;

        public float mouseSensitivity = 100f;

        private Vector2 _turn;
        private float xRotation = 0f;
        private float yRotation = 0f;


        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            TryGetComponent(out _playerWeaponController);
        }

        private void Update()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            _playerWeaponController.CurrentWeapon.Weapon.Top.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            yRotation += mouseX;
            _playerWeaponController.CurrentWeapon.Weapon.transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        }

        public void SetDirection(Vector2 dir)
        {
            _turn.x += dir.x;
            _turn.y += dir.y;
        }


    }

}