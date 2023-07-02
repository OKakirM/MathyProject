using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    private AudioSource bgAudio;
    private GameObject bg;
    public QuestionScripts question;
    public PlayerController player;
    [HideInInspector] public bool isPaused = false;

    public Animator transition;
    public float transitionTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        bg = GameObject.Find("BGMusic");
        bgAudio = bg.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !question.isSolving && !player.isDead)
        {
            bgAudio.Pause();
            if (isPaused)
            {
                bgAudio.UnPause();
                ResumeGame();
            } 
            else
            {
                bgAudio.Pause();
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        bgAudio.UnPause();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        Destroy(bg);
        StartCoroutine(LoadLevel(0));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
