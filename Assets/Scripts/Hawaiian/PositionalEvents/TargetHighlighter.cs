using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hawaiian.PositionalEvents
{
    public class TargetHighlighter : MonoBehaviour
    {
        [SerializeField] private PositionalEventCaller caller;
        
        private List<PositionalEventListener> oldTargets = new();
        
        private readonly Dictionary<PositionalEventListener, List<Highlighter>> targetHighlighters = new();

        private void OnEnable()
        {
            caller.targetsChanged.AddListener(OnTargetsChanged);
        }

        private void OnDisable()
        {
            caller.targetsChanged.RemoveListener(OnTargetsChanged);
        }

        private void OnTargetsChanged()
        {
            var newTargets = caller.Targets;
            
            foreach (var oldTarget in oldTargets)
            {
                if (!newTargets.Contains(oldTarget))
                {
                    RemoveCallerFromHighlighters(oldTarget);
                }
            }

            foreach (var newTarget in newTargets)
            {
                if (!oldTargets.Contains(newTarget))
                {
                    AddCallerToHighlighters(newTarget);
                }
            }

            // Soft copy to prevent issues
            oldTargets = newTargets.ToList();
        }

        private void AddCallerToHighlighters(PositionalEventListener target)
        {
            var highlighters = target.GetComponentsInChildren<Highlighter>().ToList();

            foreach (var highlighter in highlighters)
            {
                highlighter.AddCaller(caller);
            }

            targetHighlighters.Add(target, highlighters);
        }

        private void RemoveCallerFromHighlighters(PositionalEventListener target)
        {
            var highlighters = targetHighlighters[target];

            foreach (var highlighter in highlighters)
            {
                highlighter.RemoveCaller(caller);
            }
            
            targetHighlighters.Remove(target);
        }
    }
}