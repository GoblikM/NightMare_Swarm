using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;


    private void Start()
    {
        // Set the volume to the saved volume
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20f);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20f);
        audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SoundFXVolume")) * 20f);
    }


    public void SetMasterVolume(float volume)
    {
        //audioMixer.SetFloat("MasterVolume", volume);
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        //audioMixer.SetFloat("MusicVolume", volume);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("MusicVolume", volume);

    }

    public void SetSFXVolume(float volume)
    {
        //audioMixer.SetFloat("SoundFXVolume", volume);
        audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("SoundFXVolume", volume);
    }
}
