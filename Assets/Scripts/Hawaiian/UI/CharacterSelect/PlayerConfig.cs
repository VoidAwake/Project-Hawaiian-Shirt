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
            this.IsPlayer = false;
            this.playerNumber = playerNumber;
            this.characterNumber = -1;
        }

        public void SetPlayer(PlayerInput playerInput)
        {
            this.inputComponent = playerInput;
            this.IsPlayer = true;
        }

        public void Clear()
        {
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