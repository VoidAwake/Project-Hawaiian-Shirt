using System;
using Hawaiian.Unit;
using UnityEngine;
using TMPro;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

namespace Hawaiian.UI.CharacterSelect
{
    public class LobbyWindow : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI textNumber;
        [SerializeField] TextMeshProUGUI textPrompt;
        [SerializeField] TextMeshProUGUI textReady;
        [SerializeField] Animator unit;
        [SerializeField] SpriteResolver head;
        [SerializeField] SpriteResolver torso;
        [SerializeField] Camera renderCamera;
        [SerializeField] RawImage renderTexture;

        private LobbyPlayerController lobbyPlayerController;

        public void Initialise(LobbyPlayerController lobbyPlayerController)
        {
            this.lobbyPlayerController = lobbyPlayerController;
            
            // TODO: Unlisten?
            lobbyPlayerController.statusChanged.AddListener(OnStatusChanged);
            lobbyPlayerController.characterUpdated.AddListener(OnCharacterUpdated);
            
            OnStatusChanged();
            OnCharacterUpdated();
        }

        private void OnCharacterUpdated()
        {
            UpdateHead(lobbyPlayerController.playerConfig.characterNumber);
        }

        private void OnStatusChanged()
        {
            switch (lobbyPlayerController.Status)
            {
                case LobbyPlayerController.PlayerStatus.NotLoadedIn:
                    SetEmpty();
                    break;
                case LobbyPlayerController.PlayerStatus.LoadedIn:
                    SetUnselected();
                    break;
                case LobbyPlayerController.PlayerStatus.Ready:
                    SetSelected();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void Start()
        {
            SetEmpty();
        }

        void Update()
        {
            if (textPrompt.enabled)
            {
                textPrompt.color = new Color(textPrompt.color.r, textPrompt.color.g, textPrompt.color.b, 0.5f + 0.5f * Mathf.Sin(Time.time * 3.0f));
            }
        }

        public void SetEmpty()
        {
            textNumber.enabled = false;
            textPrompt.enabled = true;
            textReady.enabled = false;
            renderCamera.enabled = false;
            renderTexture.enabled = false;

            unit.SetFloat("speed", 0.0f);
            unit.SetBool("isRunning", false);
            unit.speed = 1.0f;
        }

        public void SetUnselected()
        {
            textNumber.enabled = true;
            textPrompt.enabled = false;
            textReady.enabled = false;
            renderCamera.enabled = true;
            renderTexture.enabled = true;

            unit.SetFloat("speed", 0.0f);
            unit.SetBool("isRunning", false);
            unit.speed = 1.0f;
        }

        public void SetSelected()
        {
            textNumber.enabled = true;
            textPrompt.enabled = false;
            textReady.enabled = true;
            renderCamera.enabled = true;
            renderTexture.enabled = true;

            unit.SetFloat("speed", 1.0f);
            unit.SetBool("isRunning", true);
            unit.speed = 5.0f;
        }

        public void UpdateHead(int characterNumber)
        {
            head.SetCategoryAndLabel("Head", ((Unit.Unit.HeadLabels)characterNumber).ToString());
        }
    }
}
