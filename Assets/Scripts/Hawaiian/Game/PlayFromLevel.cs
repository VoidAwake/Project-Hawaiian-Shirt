using Hawaiian.Game.GameModes;
using UnityEngine;

namespace Hawaiian.Game
{
    public class PlayFromLevel : MonoBehaviour
    {
        [SerializeField] private ModeManager defaultModeManager;

        // TODO: Requires manually set script execution order
        private void Awake()
        {
            Debug.LogWarning($"Playing from level. Will cause issues if left enabled.", this);
            
            defaultModeManager.SetModeAsCurrent();
            
            defaultModeManager.ListenToLevel();
        }
    }
}