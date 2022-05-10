using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.Unit
{
    [CreateAssetMenu(menuName = "Hawaiian/PlayersData")]
    public class PlayersData : ScriptableObject
    {
        public List<PlayerData> players;

        public PlayersData(List<PlayerData> players)
        {
            this.players = players;
        }
    }
}