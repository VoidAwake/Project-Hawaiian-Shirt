using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.Game
{
    [CreateAssetMenu(order = 0, menuName = "Hawaiian/GameMode")]
    public class GameModeSO : ScriptableObject
    {
        public string displayName;
        public string description;
        public List<GameModeSceneReference> sceneReferences;
        public GameObject controlsInstructionsDialoguePrefab;
        public GameObject tutorialDialoguePrefab;
        public GameObject modeControllerPrefab;
        public GameObject modeUIPrefab;
    }
}