using Fusion;
using UnityEngine;

namespace CurlyBlue
{
	public class AutoRotate : NetworkBehaviour
	{
		public float rotationSpeed = 45f;
		
		public override void FixedUpdateNetwork()
		{
			transform.Rotate(Vector3.up * rotationSpeed * Runner.DeltaTime);
		}
	}
}
