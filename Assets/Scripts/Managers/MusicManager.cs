using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField]
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null) // If instance is null, set it to this
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // If instance already exists, destroy this gameObject
        }

        DontDestroyOnLoad(gameObject); // Don't destroy this gameObject when loading a new scene
    }


    public void PlayMusic(AudioClip audioClip, float volume = 0.3f)
    {
        // check if the audio source is playing and if the clip is the same
        if (audioSource.isPlaying && audioSource.clip == audioClip)
        {
            return;
        }
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopMusic()
    {
        if(audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
