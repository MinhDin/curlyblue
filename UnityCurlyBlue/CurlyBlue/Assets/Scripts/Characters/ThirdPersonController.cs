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
        public CharacterInputData InputData;
        public CharacterControlData ControlData;// State of controller

        [Header("Settings")] 
        public float MoveSpeed = 2.0f;
        public float MoveSmoothBase     = 1.0f;
        public float MoveSmoothTime     = 1.0f;
        public float RotationSmoothTime = 0.12f;
        public float SprintSpeed        = 5.335f;
        public float GroundedOffset     = -0.14f;
        public float JumpHeight         = 1.2f;
        
        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;
        public LayerMask GroundLayers;
        
        private float _fallTimeoutDelta;
        private float _jumpTimeoutDelta;
        private float _rotationVelocity;
        
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

            if (Mathf.Abs(targetSpeed - ControlData.Speed) > MoveSmoothBase * deltaTime)
            {
                // Creates curved result rather than a linear one giving a more organic speed change
                // Flat increase
                ControlData.Speed += Mathf.Sign(targetSpeed - ControlData.Speed) * MoveSmoothBase * deltaTime;
                
                // Accelerate
                var curAccelerate = ControlData.Accelerate;
                ControlData.Speed = Mathf.SmoothDamp(ControlData.Speed, targetSpeed, ref curAccelerate, MoveSmoothTime);
                ControlData.Accelerate = curAccelerate;
            }
            else
            {
                ControlData.Speed = targetSpeed;
            }

            // Harder to turn when sprint
            var rotation = Mathf.SmoothDampAngle(ControlData.RotationY, ControlData.TargetRotationY, ref _rotationVelocity, InputData.Sprint ? RotationSmoothTime * 4 : RotationSmoothTime);
            ControlData.RotationY = rotation;
            if (InputData.MoveWorld != Vector3.zero) ControlData.TargetRotationY = Quaternion.LookRotation(InputData.MoveWorld).eulerAngles.y;
            
            Controller.Move(Quaternion.Euler(0, ControlData.RotationY, 0) * Controller.transform.forward.normalized * (ControlData.Speed * deltaTime) + new Vector3(0.0f, ControlData.SpeedY, 0.0f) * deltaTime);
        }
    }
}