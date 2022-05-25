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
    [SerializeField] private Color color;
    public Color fade;
    void Start()
    {
        //_controller = GetComponent<InventoryController>();
        _controller.currentItemChanged.AddListener(OnCurrentItemChanged);
        fade = color;
        fade = new Color(color.r, color.g, color.b, 0);

    }

    // Update is called once per frame

    private void OnCurrentItemChanged()
    {
        Debug.Log("called");
        if (_controller._inv.inv[_controller._inv.invPosition] != null)
        {
            text.text = _controller._inv.inv[_controller._inv.invPosition].ItemName;
            text.color = color;
            Debug.Log(_controller._inv.inv[_controller._inv.invPosition].ItemName);
        }
    }

    void Update()
    {
        text.color = Color.Lerp(text.color, fade, speed * Time.deltaTime);
    }
}
