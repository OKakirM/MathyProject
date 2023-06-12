using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public LevelLoader levelLoader;

    public void Play()
    {
        levelLoader.LoadNextLevel();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
