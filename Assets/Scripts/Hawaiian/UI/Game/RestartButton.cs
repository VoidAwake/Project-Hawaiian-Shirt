using Hawaiian.UI.General;
using UnityEngine.SceneManagement;

namespace Hawaiian.UI.Game
{
    public class RestartButton : Button<GameDialogue>
    {
        public override void OnClick()
        {
            base.OnClick();

            SceneManager.LoadScene(0);
        }
    }
}