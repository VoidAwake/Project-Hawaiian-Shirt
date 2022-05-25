using UnityEngine;

namespace Hawaiian.Unit
{
    [CreateAssetMenu(menuName = "Hawaiian/PlayerColours")]
    public class PlayerColors : ScriptableObject
    {
        [SerializeField] private Color[] colors;

        public Color GetColor(int playerNo)
        {
            return colors[playerNo];
        }
    }
}