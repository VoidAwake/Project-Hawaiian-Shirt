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
            SetModeAsCurrent();
            
            if (sceneReferences.Count == 0) return;

            sceneReference = sceneReferences[Random.Range(0, sceneReferences.Count)];
            
            await sceneChanger.ChangeScene(sceneReference.sceneReference);
            
            ListenToLevel();
        }
    }
}