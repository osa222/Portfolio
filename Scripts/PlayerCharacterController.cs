using UnityEngine;

namespace Battle.GamePlay
{
    /// <summary>
    /// 入力を用い武器の親オブジェクトとなるRootオブジェクトをY軸X軸に回転させるクラス
    /// </summary>
    public class PlayerCharacterController : MonoBehaviour
    {
        private PlayerWeaponController _playerWeaponController;

        public float mouseSensitivity = 100f;

        private Vector2 _turn;
        private float xRotation = 0f;
        private float yRotation = 0f;

        [SerializeField] private Transform _rotateY;
        [SerializeField] private Transform _rotateX;

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


            // Playerの武器などのX軸の回転を操作
            _rotateX.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // Playerの武器などのY軸の回転を操作
            yRotation += mouseX;
            _rotateY.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        }

        public void SetDirection(Vector2 dir)
        {
            _turn.x += dir.x;
            _turn.y += dir.y;
        }


    }

}