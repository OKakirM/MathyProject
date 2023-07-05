using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextPhase : MonoBehaviour
{
    public LevelLoader levelLoader;
    private VolumeControler volumeControler;
    public bool continueMusic;
    public bool activateQuestion;
    public QuestionScripts question;
    private GameObject bgMusic;
    private bool isTriggered;

    private void Start()
    {
        bgMusic = GameObject.Find("BGMusic");
        volumeControler = bgMusic.GetComponent<VolumeControler>();
    }

    private void Update()
    {
        if (question.correct && isTriggered)
        {
            if (!continueMusic)
            {
                Destroy(bgMusic);
            }
            levelLoader.LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (continueMusic)
            {
                DontDestroyOnLoad(bgMusic);
            }
            else
            {
                Destroy(bgMusic);
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            isTriggered = true;
            if(continueMusic)
            {
                DontDestroyOnLoad(bgMusic);
            }

            if (activateQuestion)
            {
                if(!question.isSolving)
                {
                    question.savedNumber = 0f;
                    question.Setup();
                }
            }
            else
            {
                levelLoader.LoadNextLevel();
            }
        }
        else
        {
            isTriggered = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isTriggered = false;
    }
}
