using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.UI.CharacterSelect
{
    [Serializable]
    public class PlayerConfig
    {
        public LobbyPlayerController manager { get; set; }

        public bool IsPlayer
        {
            get => isPlayer;
            set => isPlayer = value;
        }

        public int playerNumber;
        public int characterNumber;
        public float score;

        // Player input info
        public PlayerInput inputComponent;
        public int playerIndex;
        public int splitScreenIndex;
        public string controlScheme;
        public List<int> deviceIds;
        [SerializeField] private bool isPlayer;

        public PlayerConfig(int playerNumber)
        {
            this.manager = null;
            this.IsPlayer = false;
            this.playerNumber = playerNumber;
            this.characterNumber = -1;
        }

        public void SetPlayer(LobbyPlayerController LobbyPlayerController)
        {
            this.manager = LobbyPlayerController;
            this.inputComponent = LobbyPlayerController.GetComponent<PlayerInput>();
            this.IsPlayer = true;
        }

        public void Clear()
        {
            this.manager = null;
            this.IsPlayer = false;
            this.characterNumber = -1;
        }

        public void SetInputInfo(PlayerInput playerInput)
        {
            playerIndex = playerInput.playerIndex;
            splitScreenIndex = playerInput.splitScreenIndex;
            controlScheme = playerInput.currentControlScheme;
            deviceIds = playerInput.devices.Select(d => d.deviceId).ToList();
        }
    }
}