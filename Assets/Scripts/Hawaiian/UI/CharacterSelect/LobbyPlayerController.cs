using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.UI.CharacterSelect
{
    public class LobbyPlayerController : MonoBehaviour
    {
        private float move = 0;
        private bool inputEnabled;
        private int playerNumber;
        int moveBuffer;
        LobbyManager lobbyManager;
        
        // From LobbyManager.LobbyPlayer
        
        public enum PlayerStatus
        {
            NotLoadedIn,
            LoadedIn,
            LoadedInAndSelected,
            SelectedComputerPlayer
        }
        
        public PlayerStatus status;
        public LobbyWindow lobbyWindow;
        public GameObject characterSelect;
        public PlayerConfig playerConfig;
        public LobbyPlayerController lobbyPlayerController;

        public void OnCharacterSelect(InputValue value) { move = value.Get<float>(); }
        
        public void OnActionA(InputValue value)
        {
            if (!inputEnabled) return;
            
            lobbyManager.OnPlayerActionA(playerNumber, value);
        }
        
        public void OnActionB(InputValue value)
        {
            if (!inputEnabled) return;
            
            lobbyManager.OnPlayerActionB(playerNumber, value);
        }

        public void Initialise(LobbyManager lobbyManager, int playerNumber, LobbyWindow lobbyWindow, GameObject characterSelect, PlayerConfig playerConfig)
        {
            this.lobbyManager = lobbyManager;
            this.playerNumber = playerNumber;
            this.lobbyWindow = lobbyWindow;
            this.characterSelect = characterSelect;
            this.playerConfig = playerConfig;
            
            status = PlayerStatus.NotLoadedIn;
            lobbyWindow.SetEmpty();
            characterSelect.gameObject.SetActive(false);

            moveBuffer = 1;
            inputEnabled = true;
        }

        void Update()
        {
            if (moveBuffer != 0) // Reset stick input
            {
                //if (moveBuffer > 0 && move.x < 0.1f) moveBuffer = 0;
                //if (moveBuffer < 0 && move.x > -0.1f) moveBuffer = 0;
                if (move > -0.1f && move < 0.1f) moveBuffer = 0;
            }
            else //  Send stick input
            {
                if (move > 0.15f)
                {
                    lobbyManager.OnPlayerCharacterSelect(playerNumber, 1);
                    moveBuffer = 1;
                }
                if (move < -0.15f)
                {
                    lobbyManager.OnPlayerCharacterSelect(playerNumber, -1);
                    moveBuffer = -1;
                }
            }
        }
    }
}
