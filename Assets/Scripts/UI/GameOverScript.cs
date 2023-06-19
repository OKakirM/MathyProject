using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public Animator btn1, btn2, gameover;
    private bool isOn = true;

    private void Update()
    {
        btn1.SetBool("isOn", isOn);
        btn2.SetBool("isOn", isOn);
        gameover.SetBool("isOn", isOn);
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
