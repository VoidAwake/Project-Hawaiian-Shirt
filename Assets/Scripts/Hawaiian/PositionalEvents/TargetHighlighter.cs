using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hawaiian.PositionalEvents
{
    public class TargetHighlighter : MonoBehaviour
    {
        [SerializeField] private GameObject highlighterPrefab;
        
        private readonly Dictionary<PositionalEventCaller, List<PositionalEventListener>> callerCurrentTargets = new();
        private readonly Dictionary<PositionalEventListener, Highlighter> highlighters = new();

        public void OnTargetsChanged(PositionalEventCaller caller)
        {
            if (!callerCurrentTargets.ContainsKey(caller))
                callerCurrentTargets.Add(caller, new List<PositionalEventListener>());
            
            var currentTargets = callerCurrentTargets[caller];
            var newTargets = caller.Targets;
            
            foreach (var currentTarget in currentTargets)
            {
                if (!newTargets.Contains(currentTarget)) {
                    RemoveCallerFromHighlighter(caller, currentTarget);
                }
            }

            foreach (var newTarget in newTargets)
            {
                if (!currentTargets.Contains(newTarget))
                {
                    AddCallerToHighlighter(caller, newTarget);
                }
            }

            // Soft copy to prevent issues
            callerCurrentTargets[caller] = newTargets.ToList();
        }

        private void AddCallerToHighlighter(PositionalEventCaller caller, PositionalEventListener target)
        {
            Highlighter highlighter;
            
            if (!highlighters.ContainsKey(target))
            {
                var highlighterObject = Instantiate(highlighterPrefab, target.transform);

                highlighter = highlighterObject.GetComponent<Highlighter>();
                
                highlighters.Add(target, highlighter);
            }
            else
            {
                highlighter = highlighters[target];
            }
            
            highlighter.AddCaller(caller);
        }

        private void RemoveCallerFromHighlighter(PositionalEventCaller caller, PositionalEventListener target)
        {
            var highlighter = highlighters[target];
            
            highlighter.RemoveCaller(caller);

            if (highlighter.Callers.Count == 0)
            {
                Destroy(highlighter.gameObject);

                highlighters.Remove(target);
            }
        }
    }
}