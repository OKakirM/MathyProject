using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CheckAnswer : MonoBehaviour
{
    public QuestionScripts questionScript;

    public void Check()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        TextMeshProUGUI answer = button.GetComponentInChildren<TextMeshProUGUI>();

        if(answer.text == questionScript.correctAnswer.ToString())
        {
            questionScript.Correct();
        } 
        else
        {
            questionScript.Wrong();
        }
    }
}
