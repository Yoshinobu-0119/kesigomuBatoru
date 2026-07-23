using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public float screenTransitionTimer;

    public static GameManagerScript instance { get; private set; }

    private Animator anim;

    public enum GameState
    {
        TitleScreen,
        StageSelectionScreen,
        MainGame,
        ResultScreen
    }

    GameState gameState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        gameState = GameState.TitleScreen;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        DontDestroyOnLoad(gameObject);
    }

    IEnumerator titleScreenToStageSelectScreen()
    {
        yield return new WaitForSeconds(screenTransitionTimer);
    }

    IEnumerator StageSelectScreenToShibuyaStage()
    {
        yield return new WaitForSeconds(screenTransitionTimer);
    }

    IEnumerator StageSelectScreenToShiraishiStage()
    {
        yield return new WaitForSeconds(screenTransitionTimer);
    }

    IEnumerator StageSelectScreenToTakahashiStage()
    {
        yield return new WaitForSeconds(screenTransitionTimer);
    }

    IEnumerator StageSelectScreenToYuuStage()
    {
        yield return new WaitForSeconds(screenTransitionTimer);
    }

    IEnumerator MainGameToResultScreen()
    {
        yield return new WaitForSeconds(screenTransitionTimer);
    }

    IEnumerator ResultScreenToTitleScreen()
    {
        yield return new WaitForSeconds(screenTransitionTimer);
    }

    IEnumerator ResultScreenToSelectStage()
    {
        yield return new WaitForSeconds(screenTransitionTimer);
    }
}
