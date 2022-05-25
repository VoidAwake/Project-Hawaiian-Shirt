using UnityEngine;

namespace Hawaiian.Unit
{
    [CreateAssetMenu(menuName = "Hawaiian/PlayerSprites")]
    public class PlayerSprites : ScriptableObject
    {
        [SerializeField] private Sprite[] sprites;
        
        public Sprite GetSprite(int characterNo)
        {
            return sprites[characterNo];
        }
    }
}