using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

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

        public PlayerStatus Status
        {
            get => status;
            private set
            {
                status = value;
                OnStatusChanged();
            }
        }

        public LobbyWindow lobbyWindow;
        public GameObject characterSelect;
        public PlayerConfig playerConfig;
        private PlayerStatus status;

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

        private void OnEnable()
        {
            InputSystem.onEvent += OnAnyButtonPressed;
        }

        private void OnDisable()
        {
            InputSystem.onEvent -= OnAnyButtonPressed;
        }

        #endregion

        #region PlayerInput Messages

        public void OnCharacterSelect(InputValue value)
        {
            if (!inputEnabled) return;

            if (Status != PlayerStatus.LoadedIn) return;
            
            move = value.Get<float>();
        }

        public void OnActionA(InputValue value)
        {
            if (!inputEnabled) return;
            
            if (value.Get<float>() < 0.55f) return;
            
            switch (Status)
            {
                case PlayerStatus.NotLoadedIn:
                    // Handled by OnAnyButtonPressed
                    break;
                case PlayerStatus.LoadedIn:
                    Status = PlayerStatus.Ready;
                    break;
                case PlayerStatus.Ready:
                    lobbyManager.RequestStartGame();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void OnActionB(InputValue value)
        {
            if (!inputEnabled) return;

            if (value.Get<float>() < 0.55f) return;

            switch (Status)
            {
                case PlayerStatus.NotLoadedIn:
                    lobbyManager.RequestMainMenu();
                    break;
                case PlayerStatus.LoadedIn:
                    UpdateCharacterSelection(-1);
                    Status = PlayerStatus.NotLoadedIn;
                    break;
                case PlayerStatus.Ready:
                    if (lobbyManager != null)
                    {
                        Status = PlayerStatus.LoadedIn;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        // TODO: Invert the dependencies of these UI elements
        private void OnStatusChanged()
        {
            playerConfig.IsPlayer = Status != PlayerStatus.NotLoadedIn;
            
            switch (Status)
            {
                case PlayerStatus.NotLoadedIn:
                    lobbyWindow.SetEmpty();
                    characterSelect.SetActive(false);
                    // lobbyManager.GetPortrait(playerConfig.characterNumber).alpha = 1.0f;
                    inputEnabled = false;
                    break;
                case PlayerStatus.LoadedIn:
                    lobbyWindow.SetUnselected();
                    characterSelect.SetActive(true);
                    lobbyManager.GetPortrait(playerConfig.characterNumber).alpha = 1.0f;
                    StartCoroutine(EnableInput());
                    break;
                case PlayerStatus.Ready:
                    lobbyWindow.SetSelected();
                    characterSelect.SetActive(true);
                    lobbyManager.GetPortrait(playerConfig.characterNumber).alpha = 0.2f;
                    StartCoroutine(EnableInput());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            lobbyManager.UpdateReadyToStart();
        }

        public void Initialise(LobbyManager lobbyManager, LobbyWindow lobbyWindow, GameObject characterSelect, PlayerConfig playerConfig)
        {
            this.lobbyManager = lobbyManager;
            this.lobbyWindow = lobbyWindow;
            this.characterSelect = characterSelect;
            this.playerConfig = playerConfig;

            moveBuffer = 1;
            
            StartCoroutine(EnableInput());
            
            UpdateCharacterSelection(lobbyManager.FirstUnselectedCharacter());
            Status = PlayerStatus.LoadedIn;
        }

        // When players join with the ready button this prevents them from immediately being ready.
        private IEnumerator EnableInput()
        {
            yield return null;

            inputEnabled = true;
        }

        private void UpdateCharacterSelection(int charNumber)
        {
            playerConfig.characterNumber = charNumber;
            
            if (charNumber == -1) return;

            var portraitTransform = lobbyManager.GetPortrait(charNumber).GetComponent<RectTransform>();
            characterSelect.GetComponent<RectTransform>().anchoredPosition = portraitTransform.anchoredPosition;
            lobbyWindow.UpdateHead(charNumber);
        }

        private void OnPlayerCharacterSelect(int direction)
        {
            if (direction == 0) return;

            UpdateCharacterSelection(lobbyManager.FirstUnselectedCharacterFrom(playerConfig.characterNumber, direction));
        }
        
        private void OnAnyButtonPressed(InputEventPtr eventPtr, InputDevice device)
        {
            if (Status != PlayerStatus.NotLoadedIn) return;
            
            if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>()) return;

            if (!GetComponent<PlayerInput>().devices.Contains(device)) return;

            // Copied from InputUser, works somehow.
            foreach (var control in eventPtr.EnumerateChangedControls(device: device, magnitudeThreshold: 0.0001f))
            {
                if (control == null || control.synthetic || control.noisy) continue;

                // If the button pressed was the ActionB button, request to return to the main menu
                if (GetComponent<PlayerInput>().actions.FindAction("ActionB", true).controls.Contains(control))
                {
                    var requestGranted = lobbyManager.RequestMainMenu();

                    if (requestGranted)
                        break;
                }

                LoadIn();
                
                break;
            }
        }

        private void LoadIn()
        {
            UpdateCharacterSelection(lobbyManager.FirstUnselectedCharacterFrom(playerConfig.characterNumber, 1));
            Status = PlayerStatus.LoadedIn;
        }
    }
}
