using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CurlyBlue
{
    /// <summary> Read CharacterInput and move CharacterController accordingly </summary>
    public class ThirdPersonController : MonoBehaviour
    {
        public CharacterController Controller;
        
        [Header("Data")] 
        public CharacterInputData       InputData;
        public CharacterControlData ControlData;// State of controller

        [Header("Settings")] 
        public float MoveSpeed = 2.0f;
        public float MoveAccelerateSpeed = 10.0f;
        public float SprintSpeed    = 5.335f;
        public float GroundedOffset = -0.14f;
        public float JumpHeight     = 1.2f;
        
        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;
        public LayerMask GroundLayers;
        
        private float _fallTimeoutDelta;
        private float _jumpTimeoutDelta;
        
        public void ManualUpdate(float deltaTime)
        {
            JumpAndGravity(deltaTime);
            GroundedCheck(deltaTime);
            Move(deltaTime);
        }

        private void JumpAndGravity(float deltaTime)
        {
            if (ControlData.IsGrounded)
            {
                _fallTimeoutDelta = FallTimeout; // reset the fall timeout timer
                
                if (ControlData.SpeedY < 0.0f) ControlData.SpeedY = -2f; // Stop our velocity dropping infinitely when grounded

                // Check Jump
                if (InputData.Jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // The square root of H * -2 * G = how much velocity needed to reach desired height
                    ControlData.SpeedY = Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y);
                }

                if (_jumpTimeoutDelta >= 0.0f) _jumpTimeoutDelta -= deltaTime;
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout; // Reset the jump timeout timer
                InputData.Jump        = false;       // If we are not grounded, do not jump
                
                // Fall timeout
                if (_fallTimeoutDelta >= 0.0f) _fallTimeoutDelta -= deltaTime;
                else ControlData.SpeedY                          += Physics.gravity.y * deltaTime;
            }
        }

        private void GroundedCheck(float deltaTime)
        {
            var position       = transform.position;
            var spherePosition = new Vector3(position.x, position.y - GroundedOffset, position.z);
            ControlData.IsGrounded = Physics.CheckSphere(spherePosition, 0.28f, GroundLayers, QueryTriggerInteraction.Ignore);
        }
        
        private void Move(float deltaTime)
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            var targetSpeed = InputData.MoveRaw == Vector2.zero ? 0 : (InputData.Sprint ? SprintSpeed : MoveSpeed);
            
            const float speedOffset = 0.1f;

            if (ControlData.Speed < targetSpeed - speedOffset || ControlData.Speed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                ControlData.Speed = Mathf.Lerp(ControlData.Speed, targetSpeed, deltaTime * MoveAccelerateSpeed);
            }
            else
            {
                ControlData.Speed = targetSpeed;
            }
            
            if (InputData.MoveWorld != Vector3.zero) ControlData.RotationY = Quaternion.LookRotation(InputData.MoveWorld).eulerAngles.y;
            
            // move the player
            Debug.Log($"Move {InputData.MoveWorld.normalized                    * (ControlData.Speed * deltaTime) + new Vector3(0.0f, ControlData.SpeedY, 0.0f) * deltaTime}");
            Controller.Move(InputData.MoveWorld.normalized * (ControlData.Speed * deltaTime) + new Vector3(0.0f, ControlData.SpeedY, 0.0f) * deltaTime);
        }
    }
}