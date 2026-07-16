using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [Header("オブジェクト参照")]
    [SerializeField]private TextMeshProUGUI player1ScoreText;
    [SerializeField]private TextMeshProUGUI player2ScoreText;

    [Header("パラメーター")]
    public int player1Score;
    public int player2Score;

    //準備
    private void Awake()
    {   //自身をインスタンス登録
        Instance = this;
    }
    private void Start()
    {   
        //スコア初期化
        player1Score = 0;
        player2Score = 0;
        UpdateScore();
    }

    public void AddScore(bool is1p, int score)
    {
        if (is1p)
        {
            player1Score += score;
        }
        else
        {
            player2Score += score;
        }
    }


    #region ScoreDisplay

    public void UpdateScore()
    {
        if (player1ScoreText != null)
        {
            player1ScoreText.text = player1Score.ToString();
        }
        if (player2ScoreText != null)
        {
            player2ScoreText.text = player2Score.ToString();
        }
    }

    #endregion
}
