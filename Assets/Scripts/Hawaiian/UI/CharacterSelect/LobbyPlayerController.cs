using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.UI.CharacterSelect
{
    public class LobbyPlayerController : MonoBehaviour
    {
        private float move;
        private bool inputEnabled;
        int moveBuffer;
        LobbyManager lobbyManager;
        
        public enum PlayerStatus
        {
            NotLoadedIn,
            LoadedIn,
            Ready
        }
        
        public PlayerStatus status;
        public LobbyWindow lobbyWindow;
        public GameObject characterSelect;
        public PlayerConfig playerConfig;

        #region MonoBehaviour Functions

        void Update()
        {
            if (moveBuffer != 0) // Reset stick input
            {
                if (move > -0.1f && move < 0.1f) moveBuffer = 0;
            }
            else //  Send stick input
            {
                if (move > 0.15f)
                {
                    OnPlayerCharacterSelect(1);
                    moveBuffer = 1;
                }
                if (move < -0.15f)
                {
                    OnPlayerCharacterSelect(-1);
                    moveBuffer = -1;
                }
            }
        }

        #endregion

        #region PlayerInput Messages

        public void OnCharacterSelect(InputValue value)
        {
            if (!inputEnabled) return;
            
            move = value.Get<float>();
        }

        public void OnActionA(InputValue value)
        {
            if (!inputEnabled) return;
            
            if (value.Get<float>() < 0.55f) return;
            
            if (status == PlayerStatus.Ready)
            {
                lobbyManager.RequestStartGame();
            }
            else if (status == PlayerStatus.LoadedIn)
            {
                lobbyWindow.SetSelected();
                lobbyManager.GetPortrait(playerConfig.characterNumber).alpha = 0.2f;
                status = PlayerStatus.Ready;
                lobbyManager.UpdateReadyToStart();
            }
        }
        
        public void OnActionB(InputValue value)
        {
            if (!inputEnabled) return;
            
            if (value.Get<float>() < 0.55f) return;
            
            if (status == PlayerStatus.Ready)
            {
                lobbyWindow.SetUnselected();
                lobbyManager.GetPortrait(playerConfig.characterNumber).alpha = 1.0f;
                status = PlayerStatus.LoadedIn;
                lobbyManager.UpdateReadyToStart();
            }
            else if (status == PlayerStatus.LoadedIn)
            {
                // Reset visuals for current player to unloaded in versions
                lobbyWindow.SetEmpty();
                characterSelect.SetActive(false);
                lobbyManager.GetPortrait(playerConfig.characterNumber).alpha = 1.0f;
                status = PlayerStatus.NotLoadedIn;
                playerConfig.Clear();
                lobbyManager.UpdateReadyToStart();

                lobbyManager.UnloadPlayer(this);
            }
        }

        #endregion

        public void Initialise(LobbyManager lobbyManager, LobbyWindow lobbyWindow, GameObject characterSelect, PlayerConfig playerConfig)
        {
            this.lobbyManager = lobbyManager;
            this.lobbyWindow = lobbyWindow;
            this.characterSelect = characterSelect;
            this.playerConfig = playerConfig;
            
            // TODO: Overridden here, but a good reference for what need to be set
            // status = PlayerStatus.NotLoadedIn;
            // lobbyWindow.SetEmpty();
            // characterSelect.gameObject.SetActive(false);

            moveBuffer = 1;
            StartCoroutine(EnableInput());
            
            if (!playerConfig.IsPlayer) return;
            
            lobbyWindow.SetUnselected();
            characterSelect.SetActive(true);

            UpdateCharacterSelection(lobbyManager.FirstUnselectedCharacter());
            status = PlayerStatus.LoadedIn;
            
            lobbyManager.UpdateReadyToStart();
        }

        // When players join with the ready button this prevents them from immediately being ready.
        private IEnumerator EnableInput()
        {
            yield return null;

            inputEnabled = true;
        }

        private void UpdateCharacterSelection(int charNumber)
        {
            var portraitTransform = lobbyManager.GetPortrait(charNumber).GetComponent<RectTransform>();
            characterSelect.GetComponent<RectTransform>().anchoredPosition = portraitTransform.anchoredPosition;
            playerConfig.characterNumber = charNumber;
            lobbyWindow.UpdateHead(charNumber);
        }

        private void OnPlayerCharacterSelect(int direction)
        {
            if (direction == 0) return;

            UpdateCharacterSelection(lobbyManager.FirstUnselectedCharacterFrom(playerConfig.characterNumber, direction));
        }
    }
}
