using Fusion;
using UnityEngine;
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
            Initializing,
            InGame,
        }

        public NetworkedCharacter  NetworkedCharacterPrefab;
        public ThirdPersonCamera   CameraPrefab;
        public InputSystemReceiver CharacterInput;

        [field: SerializeField] public State CurrentState { get; set; }

        private ThirdPersonCamera  _camera;
        private NetworkedCharacter _mainNetworkedCharacter;

        public void PlayerJoined(PlayerRef player)
        {
            if (player == Runner.LocalPlayer)
            {
                _mainNetworkedCharacter = Runner.Spawn(NetworkedCharacterPrefab, new Vector3(0, 1, 0), Quaternion.identity);
                SetUpCharacter();
            }
        }

        public void SetUpCharacter()
        {
            _camera ??= Instantiate(CameraPrefab, transform).GetComponent<ThirdPersonCamera>();
            _camera.FollowTarget(_mainNetworkedCharacter.transform, _mainNetworkedCharacter.GetComponent<CharacterInputData>());

            CharacterInput = Instantiate(CharacterInput, _mainNetworkedCharacter.transform).GetComponent<InputSystemReceiver>();
            CharacterInput.SetData(_mainNetworkedCharacter.InputData, _camera.Camera);
        }
    }
}