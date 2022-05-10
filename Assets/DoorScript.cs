using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    Animator animator;
    public bool isLocked = true;
    [SerializeField] public bool inRange = false;
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

    public void UnlockDoor()
    {
        animator.SetTrigger("Unlock");
    }



    public void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }
    }
}
