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
        anim.Play("noTransitiom");
    }

    //--------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        //--------------------------------------------------------------
        //Important
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        DontDestroyOnLoad(gameObject);
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
