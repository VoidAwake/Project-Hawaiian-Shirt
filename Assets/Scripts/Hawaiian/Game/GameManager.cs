using Hawaiian.Utilities;
using UnityEngine;

namespace Hawaiian.Game
{
    [CreateAssetMenu(menuName = "Hawaiian/Managers/GameManager", fileName = "NewGameManager")]
    public class GameManager : ScriptableObject
    {
        public enum GamePhase { Stealth, HighAlert }

        private GamePhase phase;

        public GamePhase Phase
        {
            get => phase;
            set
            {
                phase = value;
                phaseChanged.Raise();
            }
        }

        public GameEvent phaseChanged;
    }
}