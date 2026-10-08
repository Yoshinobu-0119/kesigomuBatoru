using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultSelectanuSelect : MonoBehaviour
{
    [SerializeField]Button restart;
    [SerializeField] Button select;
    [SerializeField] Button title;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        restart.Select();
    }

    public void TitleBack()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SelectBack()
    {
        SceneManager.LoadScene("MainClassroom");
    }

}
