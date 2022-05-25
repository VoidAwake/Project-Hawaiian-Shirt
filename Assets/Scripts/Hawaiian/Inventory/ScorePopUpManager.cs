using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.Inventory
{
    public class ScorePopUpManager : MonoBehaviour
    {
        [SerializeField] GameObject popUp;

        public void InstantiateScorePopUp(Transform parent, int scoreChange)
        {
            if (scoreChange != 0)
            {
                ScorePopUp newPopUp = Instantiate(popUp, transform).GetComponent<ScorePopUp>();
                newPopUp.SetUp(scoreChange, parent);
            }
        }
    }
}
