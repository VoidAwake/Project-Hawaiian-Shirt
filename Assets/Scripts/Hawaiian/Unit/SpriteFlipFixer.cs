using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class SpriteFlipFixer : MonoBehaviour
{
    private float  originalScale;
    void Awake()
    {
        originalScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
 
    }
}
