using Hawaiian.Game.GameModes;
using UI.Core;
using UnityEngine;

namespace Hawaiian.UI.Game
{
    public abstract class ModeUI<T> : DialogueComponent<GameDialogue> where T : ModeManager
    {
        [SerializeField] protected T modeController;
        
        protected override void Subscribe() { }

        protected override void Unsubscribe() { }
    }
}