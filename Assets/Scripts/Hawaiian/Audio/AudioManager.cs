using FMOD.Studio;
using UnityEngine;

namespace Hawaiian.Audio
{
    public class AudioManager : ScriptableObject
    {
        //FMOD instance of the current music track
        public EventInstance MusicInstance;


        public EventInstance PlaySoundOneShot(string path) => FMODUnity.RuntimeManager.PlayOneShot(path);

        public EventInstance PlaySoundOneShot(AudioTrack track) => FMODUnity.RuntimeManager.PlayOneShot(track.Path);

        public EventInstance PlaySoundOneShot(string path, float volume) =>
            FMODUnity.RuntimeManager.PlayOneShot(path, volume);

        public EventInstance PlaySoundOneShot(AudioTrack track, float volume) =>
            FMODUnity.RuntimeManager.PlayOneShot(track.Path, volume);


        public EventInstance PlayMusic(string path)
        {
            MusicInstance = FMODUnity.RuntimeManager.CreateInstance(path);
            MusicInstance.start();
            MusicInstance.release();
            return MusicInstance;
        }
        
        public EventInstance PlayMusic(string path, float volume)
        {
            MusicInstance = FMODUnity.RuntimeManager.CreateInstance(path);
            MusicInstance.setVolume(volume);
            MusicInstance.start();
            MusicInstance.release();
            return MusicInstance;
        }
        

        public void StopSound(EventInstance soundInstance)
        {
            if (soundInstance.isValid())
                soundInstance.stop(STOP_MODE.IMMEDIATE);
        }
    }
}