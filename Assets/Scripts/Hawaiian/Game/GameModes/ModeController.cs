using UnityEngine;

namespace Hawaiian.Game.GameModes
{
    public abstract class ModeController<T> : MonoBehaviour, IModeController where T : ModeSceneReference
    {
        protected PlayerManager playerManager;
        protected T sceneReference;

        protected virtual void OnDestroy()
        {
            if (playerManager != null)
                playerManager.playerJoined.RemoveListener(OnPlayerJoined);
        }

        public virtual void Initialise(PlayerManager playerManager, T modeSceneReference)
        {
            this.playerManager = playerManager;
            
            playerManager.playerJoined.AddListener(OnPlayerJoined);

            sceneReference = modeSceneReference;
        }

        public virtual void SaveScores() { }

        protected virtual void OnPlayerJoined(PlayerConfig playerConfig) { }
    }
}