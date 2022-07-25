using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.Game.GameModes.TreasureHoard
{
    [CreateAssetMenu(order = 0, menuName = "Hawaiian/ModeSceneReference/TreasureHoardSceneReference")]
    public class TreasureHoardSceneReference : ModeSceneReference
    {
        public List<Vector2> treasureSpawnPoints;
    }
}