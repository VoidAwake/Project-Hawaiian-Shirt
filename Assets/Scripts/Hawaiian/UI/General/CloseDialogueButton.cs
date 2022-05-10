using UI.Core;

namespace Hawaiian.UI.General
{
    public class CloseDialogueButton : Button<Dialogue>
    {
        protected override void OnClick()
        {
            base.OnClick();
            
            manager.Pop();
        }
    }
}