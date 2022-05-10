using UI.Core;

namespace Hawaiian.UI.General
{
    public class CloseDialogueButton : Button<Dialogue>
    {
        public override void OnClick()
        {
            base.OnClick();
            
            manager.Pop();
        }
    }
}