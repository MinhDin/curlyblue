using System;
using Cinemachine;
using UnityEngine;

namespace CurlyBlue
{
    /// <summary> Rotate around character </summary>
    public class ThirdPersonCamera : MonoBehaviour
    {
        public CharacterInputData       InputData;
        public Camera                   Camera;
        public CinemachineVirtualCamera VirtualCamera;
        
        [Header("Settings")] 
        public float RotateSpeed = 1;
        public float TopClamp    = 70.0f;
        public float BottomClamp = -30.0f;

        private Transform _cameraLookAt;
        private Transform _followTarget;
        private float     _cinemachineTargetYaw;
        private float     _cinemachineTargetPitch;

        public void FollowTarget(Transform target, CharacterInputData inputData)
        {
            _followTarget = target;
            InputData = inputData;
            
            _cameraLookAt ??= new GameObject("CameraLookAt").transform;
            _cameraLookAt.SetParent(target);
            _cameraLookAt.localPosition = Vector3.zero;
            _cameraLookAt.localRotation = Quaternion.identity;
            
            VirtualCamera.Follow = _cameraLookAt;
            VirtualCamera.LookAt = _cameraLookAt;
        }

        private void LateUpdate()
        {
            if (InputData == null) return;
            
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

            _cameraLookAt.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle  -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            //Cursor.lockState = hasFocus ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}