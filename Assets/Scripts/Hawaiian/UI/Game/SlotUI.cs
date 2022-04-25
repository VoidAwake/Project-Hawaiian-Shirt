using UnityEngine;
using UnityEngine.UI;

namespace Hawaiian.UI.Game
{
    public class SlotUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Sprite def;

        public void UpdateSprite(Sprite spr = null)
        {
            if (spr != null)
            {
                image.sprite = spr;
            }
            else
            {
                image.sprite = def;
            }
        }
    }
}
