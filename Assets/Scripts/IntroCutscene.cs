using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroCutscene : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.loopPointReached += EndReached;
    }

    private void Update()
    {
        // any key press will skip the cutscene
        if (Input.anyKeyDown)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Title Screen");
        }

    }

    void EndReached(VideoPlayer vp)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title Screen");
    }


}
