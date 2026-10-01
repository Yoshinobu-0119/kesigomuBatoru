using UnityEngine.UI;
using UnityEngine;

public class PlayersStates : MonoBehaviour
{
    //Spriteを表示する
    public Image status1p;
    public Image status2p;

    public Sprite[] img;

    //プレイヤーからの取得
    public Player1Script pl1;
    public Player2Script pl2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        status1p.sprite = img[0];
        status2p.sprite = img[0];
    }

    // Update is called once per frame
    void Update()
    {
        //1pの状態（state）を取得し、imageに状態にあったイラストを表示する
        switch (pl1.state)
        {
            case Player1Script.PlayerState.None:
                status1p.sprite = img[0]; break;
            case Player1Script.PlayerState.SpeedUP:
                status1p.sprite = img[1]; break;
            case Player1Script.PlayerState.ScoreUP:
                status1p.sprite = img[2]; break;
            case Player1Script.PlayerState.Debuff:
                status1p.sprite = img[3]; break;
        }

        //2pの状態（state）を取得し、imageに状態にあったイラストを表示する
        switch (pl2.state)
        {
            case Player2Script.PlayerState.None:
                status2p.sprite = img[0]; break;
            case Player2Script.PlayerState.SpeedUP:
                status2p.sprite = img[1]; break;
            case Player2Script.PlayerState.ScoreUP:
                status2p.sprite = img[2]; break;
            case Player2Script.PlayerState.Debuff:
                status2p.sprite = img[3]; break;
        }
    }
}
