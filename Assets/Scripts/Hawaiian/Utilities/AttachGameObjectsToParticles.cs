using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;


[RequireComponent(typeof(ParticleSystem))]
public class AttachGameObjectsToParticles : MonoBehaviour
{
    public GameObject m_Prefab;

    private ParticleSystem m_ParticleSystem;
    private List<GameObject> m_Instances = new List<GameObject>();
    private ParticleSystem.Particle[] m_Particles;

    private Color32 _lightColour;
    
    public Color32 LightColour
    {
        get => _lightColour;
        set
        {
            _lightColour = value;
            Gradient grad = new Gradient();
            grad.SetKeys( new GradientColorKey[] { new GradientColorKey(_lightColour, 0.0f), new GradientColorKey(_lightColour, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) } );
            var col = m_ParticleSystem.colorOverLifetime;
            col.color = grad;

        }
    }
    void Awake()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
        m_Particles = new ParticleSystem.Particle[m_ParticleSystem.main.maxParticles];
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        int count = m_ParticleSystem.GetParticles(m_Particles);

        while (m_Instances.Count < count)
        {
           GameObject instance = Instantiate(m_Prefab, m_ParticleSystem.transform);
           instance.GetComponent<Light2D>().color = LightColour;
           m_Instances.Add(instance);
        }

        bool worldSpace = (m_ParticleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World);
        for (int i = 0; i < m_Instances.Count; i++)
        {
            if (i < count)
            {
                if (worldSpace)
                    m_Instances[i].transform.position = m_Particles[i].position;
                else
                    m_Instances[i].transform.localPosition = m_Particles[i].position;
                m_Instances[i].SetActive(true);
            }
            else
            {
                m_Instances[i].SetActive(false);
            }
        }
    }
}
