using System;
using TMPro;
using UnityEngine;

namespace CurlyBlue
{
	public class PlayerNameDisplay : MonoBehaviour
	{
		public TextMeshPro TMPName;
		public CharacterGameData CharacterData;

		private string _lastDisplayName;
		private Camera _mainCamera;
		
		public void Update()
		{
			// Rotate toward camera
			_mainCamera        ??= Camera.main;
			if (_mainCamera != null)
			{
				transform.rotation =   Quaternion.LookRotation(transform.position - _mainCamera.transform.position);
			}
			
			if (_lastDisplayName == CharacterData.Name) return;
			
			TMPName.text = CharacterData.Name;
			_lastDisplayName = CharacterData.Name;
		}
	}
}
