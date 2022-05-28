using System;
using UnityEngine;

namespace Hawaiian.Unit
{
    [CreateAssetMenu(menuName = "Hawaiian/PlayerSprites")]
    public class PlayerSprites : ScriptableObject
    {
        [SerializeField] private Sprite[] sprites;
        
        public Sprite GetSprite(int characterNo)
        {
            if (characterNo < 0 || characterNo >= sprites.Length) throw new ArgumentOutOfRangeException($"No sprite for character number {characterNo}.");
            
            return sprites[characterNo];
        }
    }
}