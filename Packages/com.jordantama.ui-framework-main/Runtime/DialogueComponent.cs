using UnityEngine;

namespace UI.Core
{
    /// <summary>
    /// A UIComponent is any UI element that exists as part of a <see cref="Core.Dialogue"/>. All UI Elements should extend this class. 
    /// </summary>
    /// <typeparam name="T">The <see cref="Core.Dialogue"/> type this UIComponent is a part of.</typeparam>
    public abstract class DialogueComponent<T> : MonoBehaviour where T : Dialogue
    {
        protected UIManager Manager;
        protected T Dialogue;
        
        
        #region MonoBehaviour
        
        private void Awake()
        {
            Manager = UIManager.Instance;
            OnComponentAwake();
        }

        private void OnEnable()
        {
            Dialogue = Manager.GetDialogue<T>();
            
            if (Dialogue)
                Subscribe();
            else
                Manager.DialogueAdded.AddListener(OnDialogueAdded);
            
            OnComponentEnabled();
        }

        private void OnDisable()
        {
            OnComponentDisabled();
            Unsubscribe();
            
            Dialogue = null;
        }

        private void Start()
        {
            OnComponentStart();
        }

        #endregion
        
        
        #region UIComponent
        
        private void OnDialogueAdded(Dialogue addedDialogue)
        {
            if (!(addedDialogue is T compatible))
                return;

            Manager.DialogueAdded.RemoveListener(OnDialogueAdded);
            Dialogue = compatible;

            Subscribe();
        }
        
        protected virtual void OnComponentAwake() {}

        protected virtual void OnComponentEnabled() {}

        protected virtual void OnComponentDisabled() {}

        protected virtual void OnComponentStart() {}

        protected abstract void Subscribe();

        protected abstract void Unsubscribe();
        
        #endregion
    }
}
