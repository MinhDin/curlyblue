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

        const   float RotationSmoothTime = 0.12f;
        private float _fallTimeoutDelta;
        private float _jumpTimeoutDelta;
        private float _rotationVelocity;
        
        void Update()
        {
            JumpAndGravity();
            GroundedCheck();
            Move();
        }

        private void JumpAndGravity()
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

                if (_jumpTimeoutDelta >= 0.0f) _jumpTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout; // Reset the jump timeout timer
                InputData.Jump        = false;       // If we are not grounded, do not jump
                
                // Fall timeout
                if (_fallTimeoutDelta >= 0.0f) _fallTimeoutDelta -= Time.deltaTime;
                else ControlData.SpeedY += Physics.gravity.y * Time.deltaTime;
            }
        }

        private void GroundedCheck()
        {
            var position       = transform.position;
            var spherePosition = new Vector3(position.x, position.y - GroundedOffset, position.z);
            ControlData.IsGrounded = Physics.CheckSphere(spherePosition, 0.28f, GroundLayers, QueryTriggerInteraction.Ignore);
        }
        
        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            var targetSpeed = InputData.MoveRaw == Vector2.zero ? 0 : (InputData.Sprint ? SprintSpeed : MoveSpeed);
            
            const float speedOffset = 0.1f;

            if (ControlData.Speed < targetSpeed - speedOffset || ControlData.Speed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                ControlData.Speed = Mathf.Lerp(ControlData.Speed, targetSpeed, Time.deltaTime * MoveAccelerateSpeed);
            }
            else
            {
                ControlData.Speed = targetSpeed;
            }
            
            if (InputData.MoveWorld != Vector3.zero) ControlData.RotationY = Quaternion.LookRotation(InputData.MoveWorld).y;
            
            var rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, ControlData.RotationY, ref _rotationVelocity, RotationSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

            // move the player
            Controller.Move(InputData.MoveWorld.normalized * (ControlData.Speed * Time.deltaTime) + new Vector3(0.0f, ControlData.SpeedY, 0.0f) * Time.deltaTime);
        }
    }
}