using System;
using FMOD.Studio;
using UnityEngine;

namespace Hawaiian.Audio
{
    public abstract class AudioBehaviour : MonoBehaviour
    {

        /// <summary>
        /// TODO: Implement search system from GDS2
        /// </summary>
        public AudioTrack CurrentTrack = new AudioTrack();

        public EventInstance CurrentInstance;
        
        //Reference to the EditorEventRef shown in the inspector
        [HideInInspector] public ScriptableObject SelectedItem;

        [HideInInspector] public AudioPlayer audioPlayer;
        [HideInInspector] public bool playOnAwake;
        [HideInInspector] public bool isGlobal;
        [HideInInspector] public bool overrideVolume;
        [HideInInspector] public float volume;


        protected virtual void Awake()
        {
            if (playOnAwake)
                TriggerSound();
        }

        protected void OnDisable()
        {
            StopSound();
        }

        protected void OnDestroy()
        {
            StopSound();
        }


        public abstract void TriggerSound();
        public abstract void StopSound();


    }
}
