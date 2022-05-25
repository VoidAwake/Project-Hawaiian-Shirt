using System;
using System.Collections;
using System.Collections.Generic;
using Hawaiian.Inventory;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

public class ItemText : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private InventoryController _controller;
    [SerializeField] private TextMeshPro text;
    [SerializeField] private float speed;
    void Start()
    {
        //_controller = GetComponent<InventoryController>();
        _controller.currentItemChanged.AddListener(OnCurrentItemChanged);
    }

    // Update is called once per frame

    private void OnCurrentItemChanged()
    {
        Debug.Log("called");
        if (_controller._inv.inv[_controller._inv.invPosition] != null)
        {
            text.text = _controller._inv.inv[_controller._inv.invPosition].ItemName;
            text.alpha = 255f;
            Debug.Log(_controller._inv.inv[_controller._inv.invPosition].ItemName);
        }
    }

    void Update()
    {
        while (text.alpha > 0)
        {
            text.alpha -= Time.deltaTime * speed;
        }
    }
}
