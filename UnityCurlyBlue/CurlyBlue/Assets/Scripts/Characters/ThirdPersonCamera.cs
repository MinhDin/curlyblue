using System;
using UnityEngine;

namespace CurlyBlue
{
    /// <summary> Rotate around character </summary>
    public class ThirdPersonCamera : MonoBehaviour
    {
        public Transform            CameraLookAt;
        public CharacterInputData   InputData;

        [Header("Settings")] 
        public float RotateSpeed = 1;
        public float TopClamp    = 70.0f;
        public float BottomClamp = -30.0f;

        private Transform _followTarget;
        private float     _cinemachineTargetYaw;
        private float     _cinemachineTargetPitch;

        public void FollowTarget(Transform target)
        {
            _followTarget = target;
            CameraLookAt.SetParent(target);
            CameraLookAt.localPosition = Vector3.zero;
            CameraLookAt.localRotation = Quaternion.identity;
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void CameraRotation()
        {
            if (InputData.Look != Vector2.zero)
            {
                _cinemachineTargetYaw   += InputData.Look.x * RotateSpeed;
                _cinemachineTargetPitch -= InputData.Look.y * RotateSpeed;
            }

            // Clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw   = ClampAngle(_cinemachineTargetYaw,   float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp,    TopClamp);

            CameraLookAt.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle  -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            Cursor.lockState = hasFocus ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}