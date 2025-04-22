using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace CurlyBlue
{
    /// <summary> Read input system and assign to Character Input </summary>
    public class InputSystemReceiver : MonoBehaviour
    {
        public CharacterInputData CharacterInputData;
        public Camera Camera; // Move input is relative to camera

        public void SetData(CharacterInputData characterInputData, Camera camera)
        {
            CharacterInputData = characterInputData;
            Camera = camera;
        }
        
        public void OnMove(InputValue value)
        {
            if(CharacterInputData == null) return;
            
            var rawInput = value.Get<Vector2>();
            CharacterInputData.MoveRaw = rawInput;
            RefreshMoveWorld();
        }

        public void OnLook(InputValue value)
        {
            if(CharacterInputData == null) return;
            
            CharacterInputData.Look = value.Get<Vector2>();
            RefreshMoveWorld();
        }

        public void OnJump(InputValue value)
        {
            if(CharacterInputData == null) return;
            CharacterInputData.Jump = value.isPressed;
        }

        public void OnSprint(InputValue value)
        {
            if(CharacterInputData == null) return;
            CharacterInputData.Sprint = value.isPressed;
        }

        private void RefreshMoveWorld()
        {
            CharacterInputData.MoveWorld = CharacterInputData.MoveRaw.x * Camera.transform.right + CharacterInputData.MoveRaw.y * Camera.transform.forward;
        }
    }
}