using System;
using Hawaiian.Utilities;
using UnityEngine;

namespace Hawaiian.Unit
{
    public class PlayerTreasure : MonoBehaviour
    {

        public Detonator DetonatorReference;
        public IUnit PlayerReference { get; set; }

        public IUnitGameEvent AddPoints;

        private float _currentDetonationTime;

        private float _detonationTime;

        //   public GameEvent 

        public void AddTreasure()
        {
            AddPoints.Raise(PlayerReference);
        }

        public void AttemptDetonate()
        {
            StartCoroutine(DetonatorReference.RunTimerCoroutine());
        }

        public void GetDetonatorReference(Tuple<IUnit,Detonator> reference)
        {
          
            if (reference.Item1 == PlayerReference)
                return;
            
            DetonatorReference = reference.Item2;
            DetonatorReference.Treasure = this;
        }

        // public v o
    }
}