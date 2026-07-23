using UnityEngine;
using UnityEngine.SceneManagement;
public class StopManager : MonoBehaviour
{
    bool isPause = false;

    public GameObject PausePanel;
    public GameObject resume;
    public GameObject title;

    public string titleScene;

    //ゲームの状態時
    public enum GameState
    {
        Opening,    // 開始演出中
        Gameplay,   // 通常プレイ中
        Paused,     // ポーズ中
        GameOver    // ゲーム終了（ゲームオーバー/クリア）
    }
    public static GameState CurrentState { get; private set; } = GameState.Gameplay;

    bool stick = false;

    int index = 0;

    void Start()
    {
        Time.timeScale = 1f;
        isPause = false;
        PausePanel.SetActive(false);
        //ゲームの状態
        CurrentState = GameState.Gameplay;

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

        //ゲームの状態
        CurrentState = GameState.Paused;
        Debug.Log("ポーズ中");

        index = 0;
        UpdateButton();
    }

    void ResumeGame()
    {
        isPause = false;
        Time.timeScale = 1f;
        PausePanel.SetActive(false);

        //ゲームの状態
        CurrentState = GameState.Gameplay;
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