using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.Inventory
{
    public class ScorePopUpManager : MonoBehaviour
    {
        [SerializeField] GameObject popUp;
        List<ScorePopUp> existingPopUps = new List<ScorePopUp>();

        public void InstantiateScorePopUp(Transform parent, int scoreChange)
        {
            if (scoreChange != 0)
            {
                ScorePopUp newPopUp = Instantiate(popUp, transform).GetComponent<ScorePopUp>();
                newPopUp.SetUp(scoreChange, parent);

                for (int i = existingPopUps.Count - 1; i >= 0; i--)
                {
                    if (existingPopUps[i].parent == parent)
                    {
                        RemoveAndDestroy(existingPopUps[i]);
                    }
                }

                existingPopUps.Add(newPopUp);
            }
        }

        public void RemoveAndDestroy(ScorePopUp toDelete)
        {
            existingPopUps.Remove(toDelete);
            Destroy(toDelete.gameObject);
        }
    }
}
