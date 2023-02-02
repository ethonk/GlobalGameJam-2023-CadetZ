using Unity.VisualScripting;
using UnityEngine;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        [Header("Sound Manager Singleton")]
        public static SoundManager Instance;

        [Header("Sound Manager Settings")]
        [SerializeField] private float volume = 1.0f;
    
        [Header("Global Audio Sources")]
        [SerializeField] private AudioSource globalSoundEffectSource;
        [SerializeField] private AudioSource globalMusicSource;

        /// <summary>
        /// Initialize the singleton instance for the SoundManager.
        /// </summary>
        private void Awake()
        {
            // if the sound manager doesn't already exist, set it to this.
            if (Instance == null) Instance = this;
            // otherwise, destroy this instance because we only need one.
            else Destroy(gameObject);
        
            // OPTIONAL: set to not destroy on load if we want sounds to carry over to the next scene.
            //DontDestroyOnLoad(gameObject);
        
            // finally, setup the other settings of the audio sources
            globalSoundEffectSource.volume = volume;
            globalMusicSource.volume = volume;
        }

        /// <summary>
        /// Play a sound effect globally.
        /// </summary>
        /// <param name="soundName"></param>
        public void PlaySound(string soundName)
        {
            globalSoundEffectSource.PlayOneShot(Resources.Load<AudioClip>(soundName));
        }

        /// <summary>
        /// Play music globally.
        /// </summary>
        /// <param name="soundName"></param>
        public void PlayMusic(string soundName)
        {
            globalMusicSource.clip = Resources.Load<AudioClip>(soundName);
            globalMusicSource.Play();
        }

        public void PlaySound3D(string soundName, Transform position, bool parented = true)
        {
            // create a new game object with an audio source in it
            Transform newAudioSource = new GameObject().transform;
            newAudioSource.AddComponent<AudioSource>();
            newAudioSource.AddComponent<DestroySoundOnComplete>();
            newAudioSource.name = "New AudioSource (Sound Effect)";
        
            // set up sound and change to 3D sound
            newAudioSource.GetComponent<AudioSource>().volume = volume;
            newAudioSource.GetComponent<AudioSource>().spatialBlend = 1.0f;
            newAudioSource.GetComponent<AudioSource>().spatialize = true;
        
            // set new source position
            newAudioSource.position = position.position;
            // and parent if its meant to be
            if (parented) newAudioSource.SetParent(position);
        
            // play the audio source
            newAudioSource.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>(soundName));
        }
    }
}
