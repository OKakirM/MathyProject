using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public Animator transition;
    private GameObject bgMusic;
    private VolumeControler volume;
    public float transitionTime = 1f;
    public Animator btn1, btn2, gameover;
    [HideInInspector] public bool isOn = true;

    private void Start()
    {
        bgMusic = GameObject.Find("BGMusic");
        volume = bgMusic.GetComponent<VolumeControler>();
    }

    private void Update()
    {
        btn1.SetBool("isOn", isOn);
        btn2.SetBool("isOn", isOn);
        gameover.SetBool("isOn", isOn);

        if (isOn)
        {
            volume.FadeOut();
        } 
        else
        {
            volume.FadeIn();
        }
    }

    public void Setup()
    {
        gameObject.SetActive(true);
        isOn = true;
    }

    public void Restart()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
        isOn = false;
    }

    public void BackToMenu()
    {
        Destroy(bgMusic);
        StartCoroutine(LoadLevel(0));
        isOn = false;
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
