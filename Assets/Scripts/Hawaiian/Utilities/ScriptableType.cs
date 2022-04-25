using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Utilities
{
    public class ScriptableType<T> : ScriptableObject
    {
        public UnityEvent valueChanged = new();
        
        [SerializeField] private T value;

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                valueChanged.Invoke();
            }
        }
    }
}