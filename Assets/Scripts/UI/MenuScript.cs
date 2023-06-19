using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public LevelLoader levelLoader;
    public Animator anim;
    private bool isOn = true;

    private void Update()
    {
        anim.SetBool("isOn", isOn);
    }

    public void Play()
    {
        levelLoader.LoadNextLevel();
        isOn = false;
    }

    public void Quit()
    {
        isOn = false;
        Application.Quit();
    }
}
