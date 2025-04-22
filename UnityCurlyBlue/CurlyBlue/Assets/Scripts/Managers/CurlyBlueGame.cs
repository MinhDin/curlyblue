using System;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;

namespace CurlyBlue
{
    /// <summary> Main game flow </summary>
    public class CurlyBlueGame : SimulationBehaviour, IPlayerJoined, IPlayerLeft
    {
        public enum State
        {
            MainMenu,
            Connecting,
            InGame,
        }

        public NetworkedCharacter  NetworkedCharacterPrefab;
        public NetworkRunner       NetworkRunner;
        public FusionBootstrap     Fusion;
        public ThirdPersonCamera   CameraPrefab;
        public InputSystemReceiver CharacterInput;
        public UIManager           UIManager;
        
        [field: SerializeField] public State CurrentState { get; set; }

        private ThirdPersonCamera  _camera;
        private NetworkedCharacter _mainNetworkedCharacter;

        void Start()
        {
            SwitchState(State.MainMenu, true);
        }

        public void PlayerJoined(PlayerRef player)
        {
            if (player == Runner.LocalPlayer)
            {
                _mainNetworkedCharacter = Runner.Spawn(NetworkedCharacterPrefab, new Vector3(0, 1, 0), Quaternion.identity);
                SetUpCharacter();
                SwitchState(State.InGame);
            }
        }
        
        public void PlayerLeft(PlayerRef player)
        {
            // Network error
            if (player == Runner.LocalPlayer)
            {
                SwitchState(State.MainMenu);
            }
        }

        public void StartGame()
        {
            Fusion.DefaultRoomName = UIManager.UIData.RoomName;
            Fusion.StartSharedClient();
        }
        
        public void SwitchState(State newState, bool force = false)
        {
            if (!force && CurrentState == newState) return;

            switch (newState)
            {
                case State.MainMenu: 
                    Cursor.lockState = CursorLockMode.None;
                    UIManager.ShowMainMenu();
                    if (NetworkRunner.IsRunning) NetworkRunner.Disconnect(NetworkRunner.LocalPlayer);
                    break;
                case State.Connecting: break;
                case State.InGame:   
                    Cursor.lockState = CursorLockMode.Locked;
                    UIManager.ShowInGameOption();
                    break;
            }
            CurrentState = newState;
        }
        
        public void SetUpCharacter()
        {
            _camera ??= Instantiate(CameraPrefab, transform).GetComponent<ThirdPersonCamera>();
            _camera.FollowTarget(_mainNetworkedCharacter.transform, _mainNetworkedCharacter.GetComponent<CharacterInputData>());

            CharacterInput = Instantiate(CharacterInput, _mainNetworkedCharacter.transform).GetComponent<InputSystemReceiver>();
            CharacterInput.SetData(_mainNetworkedCharacter.InputData, _camera.Camera);

            _mainNetworkedCharacter.GameData.Name = UIManager.UIData.PlayerName;
            _mainNetworkedCharacter.GameData.Score = 0;
            _mainNetworkedCharacter.GameData.HeadCostume = NetworkRunner.LocalPlayer.PlayerId;
        }
    }
}