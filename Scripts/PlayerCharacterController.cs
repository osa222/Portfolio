using UnityEngine;

namespace Battle.GamePlay
{
    /// <summary>
    /// ���͂�p������̐e�I�u�W�F�N�g�ƂȂ�Root�I�u�W�F�N�g��Y��X���ɉ�]������N���X
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


            // Player�̕���Ȃǂ�X���̉�]�𑀍�
            _rotateX.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // Player�̕���Ȃǂ�Y���̉�]�𑀍�
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