using UI.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hawaiian.UI.General
{
    public class SceneChangeButton : Button<Dialogue>
    {
        // TODO: Replace with scene reference
        [SerializeField] private int buildIndex;
        
        protected override void OnClick()
        {
            base.OnClick();
            
            SceneManager.LoadScene(buildIndex);
        }
    }
}