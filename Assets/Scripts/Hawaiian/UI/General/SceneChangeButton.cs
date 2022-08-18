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
            
            sceneChanger.ChangeScene(sceneReference);
        }
    }
}