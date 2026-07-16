using UnityEngine;
using UnityEngine.SceneManagement;
public class StopManager : MonoBehaviour
{
    bool isPause = false;

    public GameObject PausePanel;
    public GameObject resume;
    public GameObject title;

    public string titleScene;



    bool stick = false;

    int index = 0;

    void Start()
    {
        Time.timeScale = 1f;
        isPause = false;
        PausePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (isPause)
                ResumeGame();
            else
                PauseGame();
        }

        if (!isPause)
            return;

        float h = Input.GetAxisRaw("Horizontal1");

        if (!stick)
        {
            if (h > 0.5f)
            {
                index = 1;
                UpdateButton();
                stick = true;
            }
            else if (h < -0.5f)
            {
                index = 0;
                UpdateButton();
                stick = true;
            }
        }

        if (Mathf.Abs(h) < 0.2f)
            stick = false;

        if (Input.GetButtonDown("Attack1"))
        {
            if (index == 0)
                ResumeGame();
            else
                SceneManager.LoadScene(titleScene);
        }



    }

    void PauseGame()
    {
        isPause = true;
        Time.timeScale = 0f;

        PausePanel.SetActive(true);

        index = 0;
        UpdateButton();
    }

    void ResumeGame()
    {
        isPause = false;
        Time.timeScale = 1f;
        PausePanel.SetActive(false);
    }
    void UpdateButton()
    {
        if (index == 0)
        {
            resume.transform.localScale = Vector3.one * 1.15f;
            title.transform.localScale = Vector3.one;
        }
        else
        {
            resume.transform.localScale = Vector3.one;
            title.transform.localScale = Vector3.one * 1.15f;
        }
    }

}