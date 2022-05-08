using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    Animator animator;
    public bool isLocked = true;
    public BoxCollider2D doorRange;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        doorRange = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UnlockDoor()
    {
        if (isLocked && doorRange.IsTouchingLayers(1))
        {

        }
    }
}
