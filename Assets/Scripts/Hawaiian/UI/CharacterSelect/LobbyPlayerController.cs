using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.UI.CharacterSelect
{
    public class LobbyPlayerController : MonoBehaviour
    {
        private float move = 0;
        private bool inputEnabled;
        private int playerNumber;
        int moveBuffer;
        LobbyManager lobbyManager;

        public void OnCharacterSelect(InputValue value) { move = value.Get<float>(); }
        
        public void OnActionA(InputValue value)
        {
            if (!inputEnabled) return;
            
            lobbyManager.OnPlayerActionA(playerNumber, value);
        }
        
        public void OnActionB(InputValue value)
        {
            if (!inputEnabled) return;
            
            lobbyManager.OnPlayerActionB(playerNumber, value);
        }

        public void Initialise(LobbyManager lobbyManager, int playerNumber)
        {
            this.lobbyManager = lobbyManager;
            this.playerNumber = playerNumber;

            moveBuffer = 1;
            inputEnabled = true;
        }

        void Update()
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
                    lobbyManager.lobbyPlayers[playerNumber].stickInput = 1;
                    moveBuffer = 1;
                }
                if (move < -0.15f)
                {
                    lobbyManager.lobbyPlayers[playerNumber].stickInput = -1;
                    moveBuffer = -1;
                }
            }
        }
    }
}
