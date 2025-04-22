using Fusion;
using UnityEngine;

namespace CurlyBlue
{
	/// <summary> Game data such as score, costume </summary>
	public class CharacterGameData : NetworkBehaviour
	{
		[field: SerializeField] [Networked] public string Name { get; set; }
		[field: SerializeField] [Networked] public int Score { get; set; }
		[field: SerializeField] [Networked] public int HeadCostume { get; set; }
	}
}
