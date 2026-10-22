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
        start = GameObject.Find("/Canvas/StartButton").GetComponent<Button>();
        replace = GameObject.Find("/Canvas/TutorialButton").GetComponent<Button>();
        endGame = GameObject.Find("/Canvas/QuitButton").GetComponent<Button>();

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
