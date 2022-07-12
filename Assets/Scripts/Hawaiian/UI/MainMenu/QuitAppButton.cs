using Hawaiian.UI.General;
using UnityEngine.Device;

namespace Hawaiian.UI.MainMenu
{
    public class QuitAppButton : Button<MainMenuDialogue>
    {
        public override void OnClick()
        {
            base.OnClick();

            Application.Quit();
        }
    }
}