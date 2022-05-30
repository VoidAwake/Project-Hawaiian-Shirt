using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAnimator : MonoBehaviour
{
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D lightSource;
    [SerializeField] private float animationRange;
    [SerializeField] private float animationSpeed;
    private float baseIntensity;

    void Start()
    {
        baseIntensity = lightSource.intensity;
    }

    void Update()
    {
        lightSource.intensity = baseIntensity + animationRange * Mathf.Sin(Time.time * animationSpeed);
    }
}
