using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.Game
{
    [CreateAssetMenu(order = 0, menuName = "Hawaiian/GameMode")]
    public class GameModeSO : ScriptableObject
    {
        public GameMode gameMode;
        public string displayName;
        public string description;
        public List<SceneReference> sceneReferences;
        public GameObject controlsInstructionsDialoguePrefab;
        public GameObject tutorialDialoguePrefab;
    }
}