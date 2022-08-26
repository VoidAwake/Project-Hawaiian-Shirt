using System;
using System.Collections;
using Hawaiian.Unit;
using TMPro;
using UnityEngine;

namespace Hawaiian.Inventory
{
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
            float elapsedTimer = _timer;
        
            while (elapsedTimer > 0)
            {
                elapsedTimer -= Time.deltaTime;
                _timerText.text = ((int)elapsedTimer).ToString();
                yield return null;
            }
        
            DetonateBase();
        }

        private void DetonateBase() 
        {
            Treasure.DetonateBase();
            Destroy(gameObject);
        }
    }
}