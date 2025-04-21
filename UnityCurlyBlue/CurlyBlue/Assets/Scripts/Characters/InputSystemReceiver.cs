using UnityEngine;
using UnityEngine.InputSystem;

namespace CurlyBlue
{
    public class InputSystemReceiver : MonoBehaviour
    {
        [Header("Character Input Values")] [field: SerializeField]
        public CharacterInput CharacterInput;

        public void OnMove(InputValue   value) => CharacterInput.Move = value.Get<Vector2>();
        public void OnLook(InputValue   value) => CharacterInput.Look = value.Get<Vector2>();
        public void OnJump(InputValue   value) => CharacterInput.Jump = value.isPressed;
        public void OnSprint(InputValue value) => CharacterInput.Sprint = value.isPressed;

        private void OnApplicationFocus(bool hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
