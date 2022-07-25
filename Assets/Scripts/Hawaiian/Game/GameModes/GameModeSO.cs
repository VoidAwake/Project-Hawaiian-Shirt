using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.Game.GameModes
{
    [CreateAssetMenu(order = 0, menuName = "Hawaiian/GameMode")]
    public class GameModeSO : ScriptableObject
    {
        public string displayName;
        public string description;
        public List<ModeSceneReference> sceneReferences;
        public GameObject controlsInstructionsDialoguePrefab;
        public GameObject tutorialDialoguePrefab;
        public GameObject modeControllerPrefab;
        public GameObject modeUIPrefab;
    }
}