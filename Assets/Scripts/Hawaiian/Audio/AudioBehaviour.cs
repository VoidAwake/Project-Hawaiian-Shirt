using FMOD.Studio;
using UnityEngine;

namespace Hawaiian.Audio
{
    public abstract class AudioBehaviour : MonoBehaviour
    {
    
        /// <summary>
        /// TODO: Implement search system from GDS2
        /// </summary>
        public AudioTrack CurrentTrack { get; set; }
        public EventInstance CurrentInstance { get; set; }

        [SerializeField] internal bool playOnAwake;
        [SerializeField] internal bool isGlobal;
        [SerializeField] internal bool overrideVolume;
        [SerializeField] internal float volume;
    }
}
