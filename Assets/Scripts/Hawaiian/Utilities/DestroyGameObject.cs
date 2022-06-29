using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{

   [SerializeField] private float _time;

   public void Awake()
       => StartCoroutine(DestoryGameObjectCoroutine());

   IEnumerator DestoryGameObjectCoroutine()
   {
      yield return new WaitForSeconds(_time);
      Destroy(this);
   }
    
}
