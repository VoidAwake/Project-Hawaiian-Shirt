using Hawaiian.UI.General;
using Hawaiian.UI.MainMenu;

namespace Hawaiian.UI.PauseMenu
{
    public class ResumeButton : Button<PauseMenuDialogue>
    {
        public override void OnClick()
        {
            base.OnClick();
            
            PauseController.pausePlayer.ResumeGame();
        }
    }
}