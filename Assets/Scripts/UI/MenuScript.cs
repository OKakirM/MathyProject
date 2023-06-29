using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public LevelLoader levelLoader;
    public VolumeControler volumeControler;
    public Animator anim;
    public Animator logo;
    private bool isOn = true;

    private void Update()
    {
        anim.SetBool("isOn", isOn);
        logo.SetBool("isOn", isOn);
    }

    public void Play()
    {
        volumeControler.FadeOut();
        levelLoader.LoadNextLevel();
        isOn = false;
    }

    public void Quit()
    {
        isOn = false;
        Application.Quit();
    }
}
