using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;  //とりあえずシーン変更

public class TimerScript : MonoBehaviour
{
    public bool canCountDown;

    [SerializeField] private TextMeshProUGUI frontText;
    [SerializeField] private TextMeshProUGUI backText;
    [SerializeField] private TextMeshProUGUI centerText;

    [Header("Minutes(分)")]
    public int minutes;

    [Header("Seconds(秒)")]
    public int seconds;

    private float timer;

    // Update is called once per frame
    void Update()
    {
        //count down
        //in time
        if (minutes >= 0 && seconds >= 0 && canCountDown)
        {
            timer += Time.deltaTime;
        }
         
        //times up
        else if (minutes <= 0 && seconds <= 0 && canCountDown)
        {
            StartCoroutine(TimeUp());
        }

        //reset
        //seconds
        if (timer >= 1)
        {
            seconds--;
            timer = 0;
        }

        //minutes
        else if (seconds < 0)
        {
            minutes--;
            seconds = 59;
        }

        frontText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        backText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public IEnumerator GameStart()
    {
        int time = 3;
        for (int i = 0; i < 3; ++i)
        {
            centerText.text = time.ToString();
            yield return new WaitForSeconds(1);
            time--;
        }
        centerText.text = "GO!";

        canCountDown = true;
        StopManager stopM = GameObject.FindFirstObjectByType<StopManager>();
        stopM.StartGame();
        yield return new WaitForSeconds(1);
        centerText.text = "";
    }

    private IEnumerator TimeUp()
    {
        //即時に行う処理
        canCountDown = false;
        minutes = 0;
        seconds = 0;
        centerText.text = "FINISH!";
        StopManager stopM = GameObject.FindFirstObjectByType<StopManager>();
        stopM.EndGame();
        Debug.Log("カウントダウン停止。一度だけログが出力されていたらOK");

        //待った後に行う処理
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("TITLE");
    }
}
