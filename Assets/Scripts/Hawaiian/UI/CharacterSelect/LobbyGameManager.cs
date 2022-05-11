using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;


namespace Hawaiian.UI.CharacterSelect
{
    public class LobbyGameManager : MonoBehaviour
    {
        public PlayerConfig[] playerConfigs;

        // Start is called before the first frame update
        void Start()
        {
            // Check that this is the one and only GameManager // Also look at preload scene for creating the GameManager
            DontDestroyOnLoad(this);
            playerConfigs = new PlayerConfig[4];
            for (int i = 0; i < 4; i++)
            {
                playerConfigs[i] = new PlayerConfig(i);
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Load appropriate manager // Or, have this manager exist independently in their own scenes
            /*if (!loadedManager)
            {
                if (gameState == GameState.MainMenu)
                {
                    if (gameObject.GetComponent<MainMenuManager>() == null)
                    {
                        gameObject.AddComponent<MainMenuManager>();
                    }
                    loadedManager = true;
                }
                else if (gameState == GameState.Board1)
                {
                    loadedManager = true;
                }
            }*/
        }

        public PlayerConfig AddPlayer(LobbyPlayerController LobbyPlayerController)
        {
            for (int i = 0; i < 4; i++)
            {
                if (!playerConfigs[i].isPlayer)
                {
                    playerConfigs[i].SetPlayer(LobbyPlayerController);
                    return playerConfigs[i];
                }
            }
            return null;
        }

        public class PlayerConfig
        {
            public LobbyPlayerController manager { get; set; }
            public bool isPlayer { get; set; }
            public int playerNumber;
            public int characterNumber;
            public float score;

            // Player input info
            public PlayerInput inputComponent;
            public int playerIndex;
            public int splitScreenIndex;
            public string controlScheme;
            public List<int> deviceIds;

            public PlayerConfig(int playerNumber)
            {
                this.manager = null;
                this.isPlayer = false;
                this.playerNumber = playerNumber;
                this.characterNumber = -1;
            }
            public void SetPlayer(LobbyPlayerController LobbyPlayerController)
            {
                this.manager = LobbyPlayerController;
                this.inputComponent = LobbyPlayerController.GetComponent<PlayerInput>();
                this.isPlayer = true;
            }

            public void Clear()
            {
                this.manager = null;
                this.isPlayer = false;
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
}
