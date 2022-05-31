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

        public PlayerConfig AddPlayer(PlayerInput playerInput)
        {
            for (int i = 0; i < 4; i++)
            {
                if (!playerConfigs[i].IsPlayer)
                {
                    playerConfigs[i].SetPlayer(playerInput);
                    // TODO: Pretty sure this shouldn't be here
                    AudioManager.audioManager.PlayerAdd();
                    return playerConfigs[i];
                }
            }
            return null;
        }
    }
}
