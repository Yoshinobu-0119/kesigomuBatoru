using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    private Animator anim;

    //--------------------------------------------------------------
    [Header("ScreenEffects")]
    public float screenTransitionTimer;
    [SerializeField] private GameObject cameraObj;
    public static GameManagerScript instance { get; private set; }
    //--------------------------------------------------------------
    [Header("StageSelect")]
    public static string revengeStage;

    public Renderer[] buttons;
    public string[] sceneNames;

    int index = 0;
    bool stick = false;

    //--------------------------------------------------------------
    public enum GameState
    {
        TitleScreen,
        StageSelectionScreen,
        MainGame,
        ResultScreen
    }
    GameState gameState;
    //--------------------------------------------------------------

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        /*if(instance != null)
        {
            Destroy(gameObject);
            return;
        }*/

        instance = this;
        DontDestroyOnLoad(gameObject);

        gameState = GameState.TitleScreen;
    }

    void Start()
    {
        UpdateButton();
    }

    //--------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal1");

        if (Input.GetButtonDown("Attack1") && gameState == GameState.TitleScreen)
        {
            StartCoroutine(titleScreenToStageSelectScreen());
        }

        //--------------------------------------------------------------
        //Stage Select
        if (gameState == GameState.StageSelectionScreen)
        {
            if (!stick)
            {
                if (h > 0.5f)
                {
                    index++;

                    if (index >= buttons.Length)
                        index = 0;

                    UpdateButton();
                    stick = true;
                }
                else if (h < -0.5f)
                {
                    index--;

                    if (index < 0)
                        index = buttons.Length - 1;

                    UpdateButton();
                    stick = true;
                }
            }

            if (Mathf.Abs(h) < 0.2f)
                stick = false;

            if (Input.GetButtonDown("Attack1"))
            {
                StartCoroutine(stageSelectScreenToMainGame());
            }
        }

        //--------------------------------------------------------------
        //Important
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        DontDestroyOnLoad(gameObject);
    }

    //--------------------------------------------------------------

    void UpdateButton()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i == index)
            {
                buttons[i].material.EnableKeyword("_EMISSION");
                buttons[i].material.SetColor("_EmissionColor", Color.yellow * 5f);
            }
            else
            {
                buttons[i].material.SetColor("_EmissionColor", Color.black);
            }
        }
    }

    //--------------------------------------------------------------

    IEnumerator fadeInTransition()
    {
        anim.Play("fadeIn");
        yield return new WaitForSeconds(screenTransitionTimer);
    }
    //---------------------------------
    IEnumerator fadeOutTransition()
    {
        anim.Play("fadeOut");
        yield return new WaitForSeconds(screenTransitionTimer);
    }
    //---------------------------------
    IEnumerator titleScreenToStageSelectScreen()
    {
        gameState = GameState.StageSelectionScreen;
        yield return new WaitForSeconds(screenTransitionTimer);
    }

    IEnumerator stageSelectScreenToMainGame()
    {
        MenuManager.revengeStage = sceneNames[index];
        StartCoroutine(fadeInTransition());
        yield return new WaitForSeconds(screenTransitionTimer);
        gameState = GameState.MainGame;
        SceneManager.LoadScene(sceneNames[index]);
        StartCoroutine(fadeOutTransition());
    }
    //---------------------------------
    IEnumerator ResultScreenToSelectStage()
    {
        StartCoroutine(fadeInTransition());
        yield return new WaitForSeconds(screenTransitionTimer);
        gameState = GameState.StageSelectionScreen;
        StartCoroutine(fadeOutTransition());
    }
    //---------------------------------
    IEnumerator ResultScreenToTitleScreen()
    {
        StartCoroutine(fadeInTransition());
        yield return new WaitForSeconds(screenTransitionTimer);
        gameState = GameState.TitleScreen;
        StartCoroutine(fadeOutTransition());
    }
    //---------------------------------
}
