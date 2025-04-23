using System;
using System.Runtime.CompilerServices;
using Fusion;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace CurlyBlue
{
    /// <summary> Main game flow </summary>
    public class CurlyBlueGame : SimulationBehaviour, IPlayerJoined
    {
        public enum State
        {
            MainMenu,
            Connecting,
            InGame,
        }

        [Header("Settings")] 
        public SceneRef Map;

        public NetworkRunner   NetworkRunner;
        public FusionBootstrap Fusion;
        public UIManager       UIManager;
        public Camera          MainMenuCamera; // Clear buffer when game end 
        
        [Header("Prefabs")] 
        public NetworkedCharacter  NetworkedCharacterPrefab;
        public ThirdPersonCamera   CameraPrefab;
        public InputSystemReceiver CharacterInputPrefab;

        [field: SerializeField] public State CurrentState  { get; set; }

        private ThirdPersonCamera   _camera;
        private NetworkedCharacter  _mainNetworkedCharacter;
        private InputSystemReceiver _inputReceiver;

        void Awake()
        {
            UIManager.ShowMainMenu();
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

        public void StartGame()
        {
            SwitchState(State.Connecting);

            Fusion.DefaultRoomName = UIManager.UIData.RoomName;
            Fusion.StartSharedClient(Map);
        }

        public void SwitchState(State newState, bool force = false)
        {
            if (!force && CurrentState == newState) return;

            switch (newState)
            {
                case State.MainMenu:
                    MainMenuCamera.gameObject.SetActive(true);
                    UIManager.ShowMainMenu();
                    if (Fusion.CurrentStage == FusionBootstrap.Stage.AllConnected) Fusion.Shutdown();
                    CleanUp();
                    break;
                case State.Connecting: break;
                case State.InGame:
                    MainMenuCamera.gameObject.SetActive(false);
                    Cursor.lockState       = CursorLockMode.Locked;
                    UIManager.ShowInGameOption();
                    break;
            }

            CurrentState = newState;
        }
        
        private void SetUpCharacter()
        {
            _camera ??= Instantiate(CameraPrefab, transform).GetComponent<ThirdPersonCamera>();
            _camera.FollowTarget(_mainNetworkedCharacter.transform, _mainNetworkedCharacter.GetComponent<CharacterInputData>());

            _inputReceiver = Instantiate(CharacterInputPrefab, _mainNetworkedCharacter.transform).GetComponent<InputSystemReceiver>();
            _inputReceiver.SetData(_mainNetworkedCharacter.InputData, _camera.Camera);

            _mainNetworkedCharacter.GameData.Name        = UIManager.UIData.PlayerName;
            _mainNetworkedCharacter.GameData.HeadCostume = NetworkRunner.LocalPlayer.PlayerId;
            _mainNetworkedCharacter.GameData.Complete    = false;
            
            SceneManager.MoveGameObjectToScene(_mainNetworkedCharacter.gameObject, SceneManager.GetSceneByBuildIndex(Map.AsIndex));
        }

        private void CleanUp()
        {
            _mainNetworkedCharacter = null;
            _inputReceiver          = null;
            _camera                 = null;
            
            if (SceneManager.loadedSceneCount > 1) SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(Map.AsIndex));
        }
        
        public void BackToMainMenu() => SwitchState(State.MainMenu, true);
    }
}