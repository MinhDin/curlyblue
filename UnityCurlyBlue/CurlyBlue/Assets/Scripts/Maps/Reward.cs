using Fusion;
using UnityEngine;

namespace CurlyBlue
{
	public class Reward : NetworkBehaviour
	{
		public UIAnnouncement Announcement;
		
		public void OnTriggerEnter(Collider other)
		{
			var characterData = other.GetComponent<CharacterGameData>();
			if (characterData == null) return;
			
			if(!characterData.Complete) RpcCompleteAnnouncement(characterData.Name);
			characterData.Complete = true;
		}
		
		[Rpc(RpcSources.All, RpcTargets.All)]
		public void RpcCompleteAnnouncement(string playerName)
		{
			Announcement.Show(playerName);
		}
	}
}
