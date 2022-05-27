using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.Inventory
{
    public class SimpleAnimation : MonoBehaviour
    {
        [SerializeField] private enum AnimationType { Sine }
        [SerializeField] AnimationType animationType;
        [SerializeField] bool animateX;
        [SerializeField] float boundsX;
        [SerializeField] float rateX;
        [SerializeField] bool animateY;
        [SerializeField] float boundsY;
        [SerializeField] float rateY;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            switch (animationType)
            {
                case AnimationType.Sine:
                    transform.localPosition = new Vector2(animateX ? boundsX * Mathf.Sin(Time.time * rateX) : 0, animateY ? boundsY * Mathf.Sin(Time.time * rateY) : 0);
                    break;
                default:
                    break;
            }
        }
    }
}