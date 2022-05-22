using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.UI.CharacterSelect
{
    public class LobbyPlayerController : MonoBehaviour
    {
        private float move = 0;
        private bool inputEnabled;

        public void OnCharacterSelect(InputValue value) { move = value.Get<float>(); }
        
        public void OnActionA(InputValue value)
        {
            if (!inputEnabled) return;
            
            mainMenuManager.OnPlayerActionA(playerConfig.playerNumber, value);
        }
        
        public void OnActionB(InputValue value)
        {
            if (!inputEnabled) return;
            
            mainMenuManager.OnPlayerActionB(playerConfig.playerNumber, value);
        }

        void HandleButtonInput(float value, ref int status)
        {
            if (status == 0 && value > 0.55f) status++;         // If previously up (0) but now down, buffer an action (1)
            else if (status == 2 && value < 0.45f) status = 0;  // If previous down and already used (2) but now up, reset to being up (0)
        }

        int moveBuffer;
        LobbyManager mainMenuManager;
        LobbyGameManager gameManager;
        PlayerConfig playerConfig;

        float selfDestructTimer;
        float inputBirthTimer;

        void Start()
        {
            mainMenuManager = FindObjectOfType<LobbyManager>();
            gameManager = FindObjectOfType<LobbyGameManager>();
            if (gameManager != null && mainMenuManager != null)
            {
                playerConfig = gameManager.AddPlayer(this);
            }
            else Destroy(gameObject);

            inputBirthTimer = 0.1f;
            moveBuffer = 1;
            inputEnabled = true;
        }

        void Update()
        {
            if (inputBirthTimer <= 0.0f)
            {
                if (playerConfig != null)
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
                            mainMenuManager.lobbyPlayers[playerConfig.playerNumber].stickInput = 1;
                            moveBuffer = 1;
                        }
                        if (move < -0.15f)
                        {
                            mainMenuManager.lobbyPlayers[playerConfig.playerNumber].stickInput = -1;
                            moveBuffer = -1;
                        }
                    }
                }
            }
            else
            {
                inputBirthTimer -= Time.deltaTime;
            }

            if (playerConfig == null)
            {
                selfDestructTimer += Time.deltaTime;
                if (selfDestructTimer > 0.5f) Destroy(gameObject);
            }
        }

        public void SetPlayerConfig(PlayerConfig playerConfig)
        {
            this.playerConfig = playerConfig;
        }

        public void UnloadPlayer()
        {
            if (playerConfig != null) playerConfig.Clear();
            playerConfig = null;
            selfDestructTimer = 0.0f;
        }
    }
}
