using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public void Setup()
    {
        gameObject.SetActive(true);
    }

    public void Restart()
    {
        EditorSceneManager.LoadScene("SampleScene");
    }
}
