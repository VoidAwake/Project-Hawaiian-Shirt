using System.Collections.Generic;
using Hawaiian.Utilities;
using UnityEngine;

namespace Hawaiian.Interactables
{
    public class TargetHighlighter : MonoBehaviour
    {
        [SerializeField] private GameEvent positionalEventCallerEnabled;
        [SerializeField] private GameEvent positionalEventCallerDisabled;
        [SerializeField] private GameObject highlighterPrefab;

        private Dictionary<GameEventListener, Highlighter> highlighters = new();

        public void OnTargetAdded(GameEventListener target)
        {
            var highlighterObject = Instantiate(highlighterPrefab, target.transform);
                
            highlighters.Add(target, highlighterObject.GetComponent<Highlighter>());
        }

        public void OnTargetRemoved(GameEventListener target)
        {
            Destroy(highlighters[target].gameObject);

            highlighters.Remove(target);
        }
    }
}