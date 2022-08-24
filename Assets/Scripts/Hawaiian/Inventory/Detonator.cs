using System;
using System.Collections;
using Hawaiian.Unit;
using TMPro;
using UnityEngine;

namespace Hawaiian.Inventory
{
    public class Detonator : MonoBehaviour
    {
        [SerializeField] private GameObject _detonationCallerReference;
        
        public void OnDetonation()
        {
            Instantiate(_detonationCallerReference);
            Destroy(this.gameObject);
        }
        
        
    }
}