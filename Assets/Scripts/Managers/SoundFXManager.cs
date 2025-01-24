using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] 
    private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// This method plays the sound effect at the given position with the given volume.
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="spawnTransform"></param>
    /// <param name="volume"></param>
    public void PlaySoundFX(AudioClip audioClip, Transform spawnTransform, float volume = 1f)
    {
        // spawn the gameObject with the audio source
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        // assign the audio clip to the gameObject
        audioSource.clip = audioClip;

        // assign volume
        audioSource.volume = volume;

        // play sound
        audioSource.Play();

        // get length of the audio clip
        float clipLength = audioClip.length;

        // destroy the gameObject after the length of the audio clip
        Destroy(audioSource.gameObject, clipLength);

    }

}
