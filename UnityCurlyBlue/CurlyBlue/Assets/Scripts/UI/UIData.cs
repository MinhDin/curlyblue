using UnityEngine;

namespace CurlyBlue
{
	public class UIData : MonoBehaviour
	{
		[field: SerializeField] public string RoomName { get; set; }
		[field: SerializeField] public string PlayerName { get; set; }
	}
}
