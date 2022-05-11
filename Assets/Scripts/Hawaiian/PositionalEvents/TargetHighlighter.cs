using System.Collections.Generic;
using System.Linq;
using Hawaiian.Unit;
using UnityEngine;

namespace Hawaiian.PositionalEvents
{
    public class TargetHighlighter : MonoBehaviour
    {
        [SerializeField] private GameObject highlighterPrefab;
        // TODO: Retrieve from PlayerPrefs.
        [SerializeField] private PlayersData playersData;
        
        private readonly Dictionary<PositionalEventCaller, Dictionary<PositionalEventListener, Highlighter>> highlighters = new();

        public void OnTargetsChanged(PositionalEventCaller caller)
        {
            if (!highlighters.ContainsKey(caller))
                highlighters.Add(caller, new Dictionary<PositionalEventListener, Highlighter>());
            else
                foreach (var target in highlighters[caller].Keys.ToList())
                {
                    RemoveHighlighter(caller, target);
                }

            foreach (var target in caller.Targets)
            {
                AddHighlighter(caller, target);
            }
        }

        private void AddHighlighter(PositionalEventCaller caller, PositionalEventListener target)
        {
            var highlighterObject = Instantiate(highlighterPrefab, target.transform);

            var highlighter = highlighterObject.GetComponent<Highlighter>();

            highlighter.Initialise(playersData.players.Count, playersData.players.Select(a => a.color).ToList());
            
            highlighters[caller].Add(target, highlighter);
        }

        private void RemoveHighlighter(PositionalEventCaller caller, PositionalEventListener target)
        {
            Destroy(highlighters[caller][target].gameObject);

            highlighters[caller].Remove(target);
        }
    }
}