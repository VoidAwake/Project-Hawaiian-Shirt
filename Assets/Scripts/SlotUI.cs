using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    // Start is called before the first frame update
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
