using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioClip[] clips;
    [SerializeField] AudioSource audioSource;
    public static AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (audioManager == null)
        {
            audioManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayWeapon(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
