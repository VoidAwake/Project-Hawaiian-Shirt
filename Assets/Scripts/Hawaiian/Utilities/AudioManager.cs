using UnityEngine;

namespace Hawaiian.Utilities
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] AudioClip[] sfxClips;
        [SerializeField] AudioSource sfx;
        public static AudioManager audioManager;
        
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

        public void PlayerTrip()
        {
            sfx.PlayOneShot(sfxClips[6]);
        }
        public void PlayWeapon(int clipIndex)
        {
            sfx.PlayOneShot(sfxClips[clipIndex]);
        }
    }
}
