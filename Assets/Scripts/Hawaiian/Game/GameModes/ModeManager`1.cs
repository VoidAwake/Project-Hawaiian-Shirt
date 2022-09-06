using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Hawaiian.Game.GameModes
{
    public abstract class ModeManager<T> : ModeManager where T : ModeSceneReference
    {
        public List<T> sceneReferences;
        protected T sceneReference;

        public override async void LoadRandomLevel()
        {
            CurrentModeManager = this;
            
            if (sceneReferences.Count == 0) return;

            sceneReference = sceneReferences[Random.Range(0, sceneReferences.Count)];
            
            await sceneChanger.ChangeScene(sceneReference.sceneReference);
            
            // TODO: Duplicate code. See GameDialogue.OnEnable.
            // TODO: Come back to this
            playerManager = FindObjectOfType<PlayerManager>();

            // TODO: Unlisten?
            playerManager.playerJoined.AddListener(OnPlayerJoined);
            
            // TODO: Unlisten?
            gameOverEvent.RegisterListener(gameOver);
        }
    }
}