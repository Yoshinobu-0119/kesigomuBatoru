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

    //追加：リスタート時のステージ名取得、ステージイントロシーンに遷移する処理の実装
    //（各ステージシーンからゲーム開始しないとバグるよ　インスタンス化したScoreManagerを参照してるので...）
    public void RestartBack()
    {
        string stageName = "Intro" + ScoreManager.stageName;
        SceneManager.LoadScene(stageName);
    }
}
