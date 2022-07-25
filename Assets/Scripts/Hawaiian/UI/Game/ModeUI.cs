using Hawaiian.Game.GameModes;
using UI.Core;

namespace Hawaiian.UI.Game
{
    public abstract class ModeUI<T> : DialogueComponent<GameDialogue> where T : IModeController
    {
        protected T modeController;
        
        protected override void Subscribe() { }

        protected override void Unsubscribe() { }

        public virtual void Initialise(T modeController)
        {
            this.modeController = modeController;
        }
    }
}