using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
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
                    Status = PlayerStatus.LoadedIn;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        // TODO: Invert the dependencies of these UI elements
        private void OnStatusChanged()
        {
            switch (Status)
            {
                case PlayerStatus.NotLoadedIn:
                    lobbyWindow.SetEmpty();
                    characterSelect.SetActive(false);
                    // lobbyManager.GetPortrait(playerConfig.characterNumber).alpha = 1.0f;
                    break;
                case PlayerStatus.LoadedIn:
                    lobbyWindow.SetUnselected();
                    characterSelect.SetActive(true);
                    lobbyManager.GetPortrait(playerConfig.characterNumber).alpha = 1.0f;
                    break;
                case PlayerStatus.Ready:
                    lobbyWindow.SetSelected();
                    characterSelect.SetActive(true);
                    lobbyManager.GetPortrait(playerConfig.characterNumber).alpha = 0.2f;
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
            if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>()) return;

            if (!GetComponent<PlayerInput>().devices.Contains(device)) return;

            var controls = device.allControls;

            var buttonPressPoint = InputSystem.settings.defaultButtonPressPoint;

            for (var i = 0; i < controls.Count; ++i)
            {
                var control = controls[i] as ButtonControl;

                if (control == null || control.synthetic || control.noisy) continue;

                if (!control.ReadValueFromEvent(eventPtr, out var value) || !(value >= buttonPressPoint)) continue;

                StartCoroutine(LoadIn());

                break;
            }
        }

        // TODO: This needs another look. We're delaying this by a frame so that the OnAction functions occur before OnAnyButtonPressed
        private IEnumerator LoadIn()
        {
            if (Status != PlayerStatus.NotLoadedIn) yield break;
            
            yield return null;
            
            if (Status != PlayerStatus.NotLoadedIn) yield break;
    
            UpdateCharacterSelection(lobbyManager.FirstUnselectedCharacterFrom(playerConfig.characterNumber, 1));
            Status = PlayerStatus.LoadedIn;
        }
    }
}
