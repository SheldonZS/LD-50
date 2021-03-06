using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
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
    }

    public void onClickBios()
    {
        SceneManager.LoadScene("Bios");
    }

    public void onClickEnding()
    {
        SceneManager.LoadScene("Ending");
    }
}
