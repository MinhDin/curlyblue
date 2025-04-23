using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CurlyBlue
{
	public class UIInGameOption : UIView
	{
		public GameObject           Root;
		public GameObject           BtnOption;
		public InputActionReference Escape;
		
		public override void Show(UIManager uiManager)
		{
			base.Show(uiManager);
			Root.SetActive(false);
			BtnOption.SetActive(true);
		}

		public void Update()
		{
			if (Escape.action.triggered)
			{
				if(Root.activeSelf) OnBackClicked();
				else OnOptionClicked();
			}
		}

		public void OnOptionClicked()
		{
			Cursor.lockState = CursorLockMode.None;
			Root.SetActive(true);
			BtnOption.SetActive(false);
		}

		public void OnBackClicked()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Root.SetActive(false);
			BtnOption.SetActive(true);
		}

		public void OnDisconnectClicked()
		{
			_uiManager.Game.SwitchState(CurlyBlueGame.State.MainMenu);
		}
	}
}
