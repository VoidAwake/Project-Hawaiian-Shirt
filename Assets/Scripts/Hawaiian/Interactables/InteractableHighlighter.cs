using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.Interactables
{
    public class TargetHighlighter : MonoBehaviour
    {
        [SerializeField] private Interactor interactor;
        [SerializeField] private GameObject highlighterPrefab;

        private Dictionary<Interactable, Highlighter> highlighters = new();

        private void Awake()
        {
            interactor.targetAdded.AddListener(OnTargetAdded);
            interactor.targetRemoved.AddListener(OnTargetRemoved);
        }

        private void OnTargetAdded(Interactable target)
        {
            var highlighterObject = Instantiate(highlighterPrefab, target.transform);
                
            highlighters.Add(target, highlighterObject.GetComponent<Highlighter>());
        }

        private void OnTargetRemoved(Interactable target)
        {
            Destroy(highlighters[target].gameObject);

            highlighters.Remove(target);
        }
    }
}