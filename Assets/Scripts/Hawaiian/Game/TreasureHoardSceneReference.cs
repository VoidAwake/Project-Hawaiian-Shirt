using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.Game
{
    [CreateAssetMenu(order = 0, menuName = "Hawaiian/GameModeSceneReference/TreasureHoardSceneReference")]
    public class TreasureHoardSceneReference : GameModeSceneReference
    {
        public List<Vector2> treasureSpawnPoints;
    }
}