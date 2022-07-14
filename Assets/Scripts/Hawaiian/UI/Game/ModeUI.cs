using Hawaiian.Game;
using UI.Core;

namespace Hawaiian.UI.Game
{
    // TODO: Should be using generics
    public abstract class ModeUI : DialogueComponent<GameDialogue>
    {
        protected override void Subscribe() { }

        protected override void Unsubscribe() { }

        public virtual void Initialise(IModeController modeController) { }
    }
}