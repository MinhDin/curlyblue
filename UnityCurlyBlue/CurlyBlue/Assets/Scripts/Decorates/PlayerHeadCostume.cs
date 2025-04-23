using System;
using System.Collections.Generic;
using UnityEngine;

namespace CurlyBlue
{
	public class PlayerHeadCostume : MonoBehaviour
	{
		public CharacterGameData GameData;
		public Transform         RootDisplay;
		public Transform         HeadDisplay;
		public List<GameObject>  HeadPrefabs;

		private GameObject _curHead;

		void Update()
		{
			if (_curHead != null) return;
			
			_curHead = Instantiate(HeadPrefabs[GameData.HeadCostume % HeadPrefabs.Count], RootDisplay, false);
			_curHead.transform.SetParent(HeadDisplay, true); // Deal with weird pivot of the model
		}
	}
}
