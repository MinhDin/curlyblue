using Fusion;
using UnityEngine;

namespace CurlyBlue
{
	public class AutoMove : NetworkBehaviour
	{
		public Vector3 From;
		public Vector3 To;
		public float Time;

		private float _curTime;
		private int _direction = 1;
		
		public override void FixedUpdateNetwork()
		{
			_curTime += _direction / Time * Runner.DeltaTime;
			if (_curTime is >= 1 or <= 0)
			{
				_direction *= -1;
			}
			
			_curTime = Mathf.Clamp(_curTime, 0, 1);
			transform.position = Vector3.Lerp(From, To, _curTime);
		}
	}
}
