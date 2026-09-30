using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    public TextMeshProUGUI winner;


    void Start()
    {
        if (ScoreManager.Instance != null)
        {
            int scoreP1 = ScoreManager.Instance.player1Score;
            int scoreP2 = ScoreManager.Instance.player2Score;

            if (scoreP1 < scoreP2)
            {
                winner.text = "Player 2 Win!";
            }
            else if (scoreP2 < scoreP1)
            {
                winner.text = "Player 1 Win!";
            }
            else
            {
                winner.text = "Player 1 & 2 Win!";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Attack1") || Input.GetButtonDown("Attack2"))
        {
            SceneManager.LoadScene("MainClassroom");
        }



    }
}
