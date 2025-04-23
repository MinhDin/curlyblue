using Fusion;
using UnityEngine;

namespace CurlyBlue
{
    /// <summary> Data for character controller </summary>
    public class CharacterControlData : NetworkBehaviour
    {
        [field: SerializeField] [Networked] public bool  IsGrounded { get; set; }
        [field: SerializeField] [Networked] public float Speed      { get; set; }
        [field: SerializeField] [Networked] public float SpeedY     { get; set; }
        [field: SerializeField] [Networked] public float Accelerate { get; set; }
        [field: SerializeField] [Networked] public float TargetRotationY  { get; set; } // Have target because we have to reach target
        [field: SerializeField] [Networked] public float RotationY  { get; set; }
    }
}