using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] sfxClips;
    [SerializeField] AudioSource uiSfx;
    [SerializeField] AudioSource sfx;
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

    public void Swap()
    {
        
            uiSfx.PlayOneShot(sfxClips[1]);
    }

    public void Confirm()
    {
        if (!uiSfx.isPlaying)
            uiSfx.PlayOneShot(sfxClips[3]);
    }

    public void PlayerAdd()
    {
        uiSfx.PlayOneShot(sfxClips[0]);
    }
    void PlayWeapon(AudioClip clip)
    {
        sfx.PlayOneShot(clip);
    }
}
