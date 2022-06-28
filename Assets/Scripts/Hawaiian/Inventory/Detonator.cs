using System;
using System.Collections;
using System.Collections.Generic;
using Hawaiian.Inventory;
using Hawaiian.Unit;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Detonator : MonoBehaviour
{
    [SerializeField] private int _timer;
    [SerializeField] private TextMeshProUGUI _timerText;


    private IUnit _player;
    public IUnit PlayerReference
    {
        get => _player;
        set
        {
            _player = value;
            ParseData(_player);
        }
    }
    public DetonatorGameEvent ParseDetantorToTreasure;

    public PlayerTreasure Treasure;

    private void Awake()
    {
        StartCoroutine(RunTimerCoroutine());
    }

    public void ParseData(IUnit player)
    {
        Tuple<IUnit, Detonator> data = new Tuple<IUnit, Detonator>(player, this);
        
        ParseDetantorToTreasure.Raise(data);

    }

    public  IEnumerator RunTimerCoroutine()
    {
        int _elapsedTimer = 0;
        
        while (_elapsedTimer < _timer)
        {
            _elapsedTimer += ( 5 - (int) Mathf.Lerp(_elapsedTimer, _timer, _elapsedTimer/_timer));
            _timerText.text = _elapsedTimer.ToString();
            yield return null;
        }
        
        DetonateBase();
    }

    private void DetonateBase()
    {
        Destroy(gameObject);
    }
}