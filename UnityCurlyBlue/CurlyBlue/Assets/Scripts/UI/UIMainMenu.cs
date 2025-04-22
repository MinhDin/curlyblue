using Fusion;
using UnityEngine;

namespace CurlyBlue
{
	public class UIMainMenu : UIView
	{
		public void OnStartGame()
		{
			_uiManager.Game.StartGame();
		}

		public void OnRoomNameChange(string roomName)
		{
			_uiManager.UIData.RoomName = roomName;
		}

		public void OnPlayerNameChange(string playerName)
		{
			_uiManager.UIData.PlayerName = playerName;
		}
	}
}
