using Fusion;
using UnityEngine;

namespace CurlyBlue
{
	public class NetworkedCharacter : NetworkBehaviour
	{
		public CharacterInputData    InputData;
		public CharacterControlData  ControlData;
		public ThirdPersonController Controller;
		
		public override void FixedUpdateNetwork()
		{
			Controller.ManualUpdate(Runner.DeltaTime);
		}
	}
}
