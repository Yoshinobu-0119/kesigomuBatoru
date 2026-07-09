using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    private enum GameState
    {
        TitleScreen,
        StageSelectionScreen,
        MainGame
    }

    GameState gameState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        gameState = GameState.TitleScreen;
    }

    void Start()
    {
        
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
}
