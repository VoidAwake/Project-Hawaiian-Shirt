using Hawaiian.Game;
using UI.Core;
using UnityEngine;

namespace Hawaiian.UI.General
{
    public class SceneChangeButton : Button<Dialogue>
    {
        [SerializeField] private SceneReference sceneReference;
        [SerializeField] private SceneChanger sceneChanger;

        public override void OnClick()
        {
            base.OnClick();
            
            // TODO: Needs revisiting. Prevents issues when spamming button presses.
            // TODO: Maybe this should be part of the SceneChanger instead?
            dialogue.Disable();
            
            sceneChanger.ChangeScene(sceneReference);
        }
    }
}