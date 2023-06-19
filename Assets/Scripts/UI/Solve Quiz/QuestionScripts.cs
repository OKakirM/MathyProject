using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionScripts : MonoBehaviour
{
    #region Buttons
    public List<GameObject> options;
    #endregion

    #region Components Variables
    public TextMeshProUGUI questionText;
    public GameOverScript gameover;
    public PlayerController playerController;
    public GameObject deathBG;
    public GameObject ui;
    #endregion

    #region Timer
    public Slider timerBar;
    public TextMeshProUGUI timerText;
    public float cooldownQuizTime;
    public float duration;

    private bool stopTimer;
    private float counter;
    [HideInInspector] public bool wasSaved = false;
    private float cooldownCounter;

    #endregion

    #region Others
    [HideInInspector] public bool isSolving = false;
    [HideInInspector] public float correctAnswer;
    #endregion

    private string question;

    private void Start()
    {
        counter = duration;
        stopTimer = false;
        timerBar.maxValue = duration;
        timerBar.value = duration;
    }

    public void UpdateQuiz()
    {
        if (isSolving && !wasSaved)
        {
            Timer();
        }

        if(wasSaved)
        {
            cooldownCounter += Time.deltaTime;
        }
    }

    public void Setup()
    {
        if (wasSaved)
        {
            if(cooldownCounter >= cooldownQuizTime)
            {
                wasSaved = false;
                cooldownCounter = 0f;
                CreateQuestion();
                SelectingRightButton();
                gameObject.SetActive(true);
                questionText.text = question;
                Time.timeScale = 0.05f;
                Time.fixedDeltaTime = Time.timeScale * 0.01f;
                isSolving = true;
            }
            else
            {
                Wrong();
            }
        } 
        else
        {
            CreateQuestion();
            SelectingRightButton();
            gameObject.SetActive(true);
            questionText.text = question;
            Time.timeScale = 0.05f;
            Time.fixedDeltaTime = Time.timeScale * 0.01f;
            isSolving = true;
        }
    }

    public void Correct()
    {
        gameObject.SetActive(false);
        playerController.currentHealth += 10;
        isSolving = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    public void Wrong()
    {
        isSolving = false;
        gameObject.SetActive(false);
        ui.SetActive(false);
        deathBG.SetActive(true);
        gameover.Setup();
        playerController.isDead = true;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    public void CreateQuestion()
    {
        float a = Random.Range(0, 10);
        float b = Random.Range(0, 10);
        int op = Random.Range(1, 5);

        if((b == 0 || a == 0) && op == 4)
        {
            b = Random.Range(1, 10);
            a = Random.Range(1, 10);
        }

        //Somar
        if(op == 1)
        {
            question = a + "+" + b + "=x";
            correctAnswer = a + b;
        }
        //Subtrair
        else if(op == 2)
        {
            question = a + "-" + b + "=x";
            correctAnswer = a - b;
        }
        //Multiplicar
        else if(op == 3)
        {
            question = a + "x" + b + "=x";
            correctAnswer = a * b;
        }
        //Dividir
        else if(op == 4)
        {
            question = a + "/" + b + "=x";
            correctAnswer = a / b;
        }
    }

    public void SelectingRightButton()
    {
        int i = 0;
        int range = Random.Range(0, options.Capacity);
        for(i = 0; i < options.Capacity; i++)
        {
            if(i == range)
            {
                options[i].name = "Correct Option";
                TextMeshProUGUI correctOption = options[i].GetComponentInChildren<TextMeshProUGUI>();
                correctOption.text = correctAnswer.ToString();
            } 
            else
            {
                options[i].name = "Wrong Option";
                TextMeshProUGUI wrongOption = options[i].GetComponentInChildren<TextMeshProUGUI>();
                wrongOption.text = (correctAnswer + Random.Range(1, 20)).ToString();
            }
        }
    }

    private void Timer()
    {
        counter -= 0.016f;

        int minutes = Mathf.FloorToInt(counter / 60);
        int seconds = Mathf.FloorToInt(counter - minutes * 60);

        string textTime = string.Format("{0}s", seconds);
        
        if(counter <= 0f)
        {
            stopTimer = true;
            counter = duration;
            isSolving = false;
            Wrong();
        }

        if (stopTimer == false)
        {
            timerText.text = textTime;
            timerBar.value = counter;
        }

    }
}
