using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManuSelect : MonoBehaviour
{
    Button start;
    Button replace;
    Button endGame;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        start = GameObject.Find("/Canvas/Button1").GetComponent<Button>();
        replace = GameObject.Find("/Canvas/Button2").GetComponent<Button>();
        endGame = GameObject.Find("/Canvas/Button3").GetComponent<Button>();

        start.Select();
    }

    public void Start_Button()
    {
        SceneManager.LoadScene("MainClassroom");
    }

    public void Tutorial_GO()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void EndGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

}
