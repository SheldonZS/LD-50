using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void onClickStart()
    {
        SceneManager.LoadScene("GameScene");
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
}
