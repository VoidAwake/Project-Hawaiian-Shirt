using Hawaiian.UI.CharacterSelect;
using Hawaiian.Utilities;
using UI.Core;

namespace Hawaiian.UI.Tutorial
{
    public class TutorialDialogue : Dialogue
    {

        public GameEvent TutorialClosedEvent;

        protected override void OnClose()
        {
           TutorialClosedEvent.Raise();
        }

        protected override void OnPromote() { }

        protected override void OnDemote() { }
    }
}