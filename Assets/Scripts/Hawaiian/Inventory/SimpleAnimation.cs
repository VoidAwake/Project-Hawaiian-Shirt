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

        void Update()
        {
            switch (animationType)
            {
                case AnimationType.Sine:
                    transform.localPosition = new Vector2(animateX ? boundsX * Mathf.Sin(Time.time * rateX) : transform.localPosition.x, animateY ? boundsY * Mathf.Sin(Time.time * rateY) : transform.localPosition.y);
                    break;
                default:
                    break;
            }
        }
    }
}