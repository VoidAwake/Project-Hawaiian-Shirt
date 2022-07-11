using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.Game
{
    [CreateAssetMenu(menuName = "Hawaiian/Managers/PlayerConfigManager")]
    public class PlayerConfigManager : ScriptableObject
    {
        [SerializeField] private int maxPlayers;
        
        public PlayerConfig[] playerConfigs;

        public PlayerConfig AddPlayer(PlayerInput playerInput)
        {
            for (int i = 0; i < maxPlayers; i++)
            {
                if (!playerConfigs[i].IsPlayer)
                {
                    playerConfigs[i].SetPlayer(playerInput);
                    // TODO: Pretty sure this shouldn't be here
                    AudioManager.audioManager.PlayerAdd();
                    ForceSerialization();
                    return playerConfigs[i];
                }
            }
            return null;
        }

        public void Clear()
        {
            playerConfigs = new PlayerConfig[maxPlayers];
            
            for (int i = 0; i < maxPlayers; i++)
            {
                playerConfigs[i] = new PlayerConfig(i);
            }
        }
        
        void ForceSerialization()
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        } 
    }
}
