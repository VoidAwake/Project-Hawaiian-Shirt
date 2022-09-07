using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hawaiian.PositionalEvents
{
    public class TargetHighlighter : MonoBehaviour
    {
        private readonly Dictionary<PositionalEventCaller, List<PositionalEventListener>> callerCurrentTargets = new();
        private readonly Dictionary<PositionalEventListener, List<Highlighter>> targetHighlighters = new();

        public void OnTargetsChanged(PositionalEventCaller caller)
        {
            if (!callerCurrentTargets.ContainsKey(caller))
                callerCurrentTargets.Add(caller, new List<PositionalEventListener>());
            
            var currentTargets = callerCurrentTargets[caller];
            var newTargets = caller.Targets;
            
            foreach (var currentTarget in currentTargets)
            {
                if (!newTargets.Contains(currentTarget))
                {
                    RemoveCallerFromHighlighters(caller, currentTarget);
                }
            }

            foreach (var newTarget in newTargets)
            {
                if (!currentTargets.Contains(newTarget))
                {
                    AddCallerToHighlighters(caller, newTarget);
                }
            }

            // Soft copy to prevent issues
            callerCurrentTargets[caller] = newTargets.ToList();
        }

        private void AddCallerToHighlighters(PositionalEventCaller caller, PositionalEventListener target)
        {
            List<Highlighter> highlighters;
            
            if (!targetHighlighters.ContainsKey(target))
            {
                highlighters = target.GetComponentsInChildren<Highlighter>().ToList();

                targetHighlighters.Add(target, highlighters);
            }
            else
            {
                highlighters = targetHighlighters[target];
            }

            foreach (var highlighter in highlighters)
            {
                highlighter.AddCaller(caller);
            }
        }

        private void RemoveCallerFromHighlighters(PositionalEventCaller caller, PositionalEventListener target)
        {
            var highlighters = targetHighlighters[target];

            foreach (var highlighter in highlighters)
            {
                highlighter.RemoveCaller(caller);
            }
            
            // TODO: I'm not sure if we should clear this. A cache without an expiration policy is a memory leak.
            // targetHighlighters.Remove(target);
        }
    }
}