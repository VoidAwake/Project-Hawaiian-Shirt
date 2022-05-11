using UI.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hawaiian.UI.General
{
    public class SceneChangeButton : Button<Dialogue>
    {
        // TODO: Replace with scene reference
        [SerializeField] private int buildIndex;

        public override void OnClick()
        {
            base.OnClick();
            
            SceneManager.LoadScene(buildIndex);
        }
    }
}