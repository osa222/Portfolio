using UnityEngine;
using UnityEngine.InputSystem;

namespace Battle.Game
{

    public class InputEventProvider : MonoBehaviour, IInputEventProvider
    {
        public Vector2 Turn => _turn;
        private Vector2 _turn;

        private void Update()
        {
            _turn.x += Input.GetAxis("Mouse X");
            _turn.y += Input.GetAxis("Mouse Y");
        }
    }

}