using UnityEngine;

namespace Hawaiian.UI.Game
{
    [CreateAssetMenu(order = 0, menuName = "Hawaiian/GameMode")]
    public class GameModeSO : ScriptableObject
    {
        public GameMode gameMode;
        public string displayName;
        public string description;
        public SceneReference sceneReference;
    }
}