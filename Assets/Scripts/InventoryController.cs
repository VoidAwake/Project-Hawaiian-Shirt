using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject UI;
    [SerializeField] private Canvas canvas;
    private GameObject reference;
    private Inventory _inv;
    [SerializeField] private bool addinv;
    [SerializeField] private Item item;
    //[SerializeField] private int invSize;
    void Awake()
    {
        _inv = ScriptableObject.CreateInstance<Inventory>();
        reference = Instantiate(UI,canvas.transform);
        reference.GetComponent<InventoryUI>().inv = _inv;
        addinv = false;
    }

    private void Update()
    {
        if (addinv)
        {
            _inv.PickUp(item);
            addinv = !addinv;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("boom boom powww");
        if (col.gameObject.tag == "Item")
        {
            if (_inv.PickUp(col.gameObject.GetComponent<tempItem>().item))
            {
                Destroy(col.gameObject);
            }
        }
    }

    // Update is called once per frame

}
