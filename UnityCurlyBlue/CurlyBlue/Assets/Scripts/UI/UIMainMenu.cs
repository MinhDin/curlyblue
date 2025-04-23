using Fusion;
using TMPro;
using UnityEngine;

namespace CurlyBlue
{
	public class UIMainMenu : UIView
	{
		public TMP_InputField RoomInputField;
		public TMP_InputField NameInputField;
		
		public void OnStartGame()
		{
			_uiManager.Game.StartGame();
			OnRoomNameChange(RoomInputField.text);
			OnPlayerNameChange(NameInputField.text);
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
