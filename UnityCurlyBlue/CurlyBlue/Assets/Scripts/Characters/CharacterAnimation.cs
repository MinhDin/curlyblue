using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CurlyBlue
{
    /// <summary> Play animation and listen to sound event </summary>
    public class CharacterAnimation : MonoBehaviour
    {
        public CharacterControlData ControlData;
        public Animator             Animator;
        public GameObject           DisplayRoot;
        public AudioClip            LandingAudioClip;
        public AudioClip[]          FootstepAudioClips;

        // Animation IDs
        private int _animIDSpeed;
        private int _animIDSpeedY;
        private int _animIDGrounded;

        const   float RotationSmoothTime = 0.12f;
        private float _rotationVelocity;
        
        void Awake()
        {
            _animIDSpeed    = Animator.StringToHash("Speed");
            _animIDSpeedY   = Animator.StringToHash("SpeedY");
            _animIDGrounded = Animator.StringToHash("Grounded");
        }

        void LateUpdate()
        {
            // Update rotation
            var rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, ControlData.RotationY, ref _rotationVelocity, RotationSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            
            // Update animation
            Animator.SetFloat(_animIDSpeed,  ControlData.Speed);
            Animator.SetFloat(_animIDSpeedY, ControlData.SpeedY);
            Animator.SetBool(_animIDGrounded, ControlData.IsGrounded);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.position, 0.5f);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.position, 0.5f);
            }
        }
    }
}