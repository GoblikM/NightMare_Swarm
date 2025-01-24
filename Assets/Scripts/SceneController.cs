using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private AudioClip music;

    public void Start()
    {
        MusicManager.instance.PlayMusic(music, 0.5f);
    }

    public void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1; // Reset the time scale
        MusicManager.instance.PlayMusic(music, 0.5f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
