using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance = null;

    private MusicLooper musicLooper;

    private void Awake()
    {
        musicLooper = GameObject.Find("BGM").GetComponent<MusicLooper>();

    }

    private void Start()
    {

    }

    public void onClickStart()
    {

        SceneManager.LoadScene("RTS");

    }

    public void onClickCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void onClickJukebox()
    {
        SceneManager.LoadScene("Jukebox");
    }

    public void onClickTitle()
    {
        SceneManager.LoadScene("Title");
        if (SceneManager.GetActiveScene().name == "Ending" || SceneManager.GetActiveScene().name == "RTS")
        {
            AudioSource bgm = musicLooper.bgm;

            if (bgm.clip.ToString() != "#50_MenuLoop (UnityEngine.AudioClip)")
            {
                musicLooper.PlayMenuLoop();
            }
        }
    }

    public void onClickBios()
    {
        SceneManager.LoadScene("Bios");
    }

    public void onClickEnding()
    {
        if (SceneManager.GetActiveScene().name != "Title")
        {
            musicLooper.bgm.Stop();
        }
        SceneManager.LoadScene("Ending");
    }

    public void onClickQuit()
    {
        Application.Quit();
    }
}
