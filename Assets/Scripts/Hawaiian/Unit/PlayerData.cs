using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.Unit
{
    [CreateAssetMenu(menuName = "Hawaiian/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public Sprite sprite;
        public Color color;
        public float score;
        
        // Input Device
        public int playerIndex;
        public int splitScreenIndex;
        public string controlScheme;
        public List<int> deviceIds;

        public PlayerData(Sprite sprite, Color color, float score, int playerIndex, int splitScreenIndex, string controlScheme, List<int> deviceIds)
        {
            this.sprite = sprite;
            this.color = color;
            this.score = score;
            this.playerIndex = playerIndex;
            this.splitScreenIndex = splitScreenIndex;
            this.controlScheme = controlScheme;
            this.deviceIds = deviceIds;
        }
    }
}