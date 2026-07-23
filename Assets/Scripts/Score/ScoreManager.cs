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

    public Player1Script pl1;
    public Player2Script pl2;


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
            if (pl1.state == Player1Script.PlayerState.ScoreUP)
            {
                player1Score += score * 2;
            }
            else
            {
                player1Score += score;
            }
        }
        else
        {
            if (pl2.state == Player2Script.PlayerState.ScoreUP)
            {
                player2Score += score * 2;
            }
            else
            {
                player2Score += score;
            }
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
