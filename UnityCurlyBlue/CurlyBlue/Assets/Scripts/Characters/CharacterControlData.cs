using UnityEngine;

namespace CurlyBlue
{
	/// <summary> Data for character controller </summary>
	public class CharacterControlData : MonoBehaviour
	{
		[field: SerializeField] public bool IsGrounded { get; set; }
		[field: SerializeField] public float Speed { get; set; }
		[field: SerializeField] public float SpeedY { get; set; }
		[field: SerializeField] public Vector3 Accelerate { get; set; }
		[field: SerializeField] public float RotationY { get; set; }
	}
}
