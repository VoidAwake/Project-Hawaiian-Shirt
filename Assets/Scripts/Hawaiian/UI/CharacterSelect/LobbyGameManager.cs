using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.UI.CharacterSelect
{
    public class LobbyGameManager : MonoBehaviour
    {
        public PlayerConfig[] playerConfigs;

        void Start()
        {
            // Ensure that this is the one and only GameManager
            LobbyGameManager[] lobbyGameManagers = FindObjectsOfType<LobbyGameManager>();
            foreach (LobbyGameManager lobbyGameManager in lobbyGameManagers)
            {
                if (lobbyGameManager != this) Destroy(lobbyGameManager.gameObject);
            }

            DontDestroyOnLoad(this);
            playerConfigs = new PlayerConfig[4];
            for (int i = 0; i < 4; i++)
            {
                playerConfigs[i] = new PlayerConfig(i);
            }
        }

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

        public PlayerConfig AddPlayer(PlayerInput playerInput)
        {
            for (int i = 0; i < 4; i++)
            {
                if (!playerConfigs[i].IsPlayer)
                {
                    playerConfigs[i].SetPlayer(playerInput);
                    AudioManager.audioManager.PlayerAdd();
                    return playerConfigs[i];
                }
            }
            return null;
        }
    }
}
