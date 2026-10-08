using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;  //とりあえずシーン変更

public class TimerScript : MonoBehaviour
{
    public bool canCountDown;

    [SerializeField] private TextMeshProUGUI frontText;
    [SerializeField] private TextMeshProUGUI backText;
    //[SerializeField] private TextMeshProUGUI centerText;

    private int seconds;
    private int minutes;

    [Header("Seconds(秒)")]
    public float time;
    //private float timeFlow;

    public GameObject resultPanel;
    private void Awake()
    {
        resultPanel = GameObject.Find("ResultPanel");
    }

    private void Start()
    {
        resultPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        minutes = Mathf.CeilToInt(time) / 60;
        seconds = Mathf.CeilToInt(time) % 60;

        //count down
        //in time
        if (time >= 0 && canCountDown)
        {
            time -= Time.deltaTime;
        }
         
        //times up
        else if (time <= 0 && canCountDown)
        {
            StartCoroutine(TimeUp());
        }

        frontText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        backText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void CountDown() //Animation Event
    {
        canCountDown = true;
        StopManager stopM = GameObject.FindFirstObjectByType<StopManager>();
        stopM.StartGame();
    }

    /*public IEnumerator GameStart()
    {
        *//* int time = 3;
         for (int i = 0; i < 3; ++i)
         {
             centerText.text = time.ToString();
             yield return new WaitForSeconds(1);
             time--;
         }
         centerText.text = "GO!";*//*

        canCountDown = true;
        StopManager stopM = GameObject.FindFirstObjectByType<StopManager>();
        stopM.StartGame();
        //yield return new WaitForSeconds(1);
        //centerText.text = "";
    }*/

    private IEnumerator TimeUp()
    {
        //即時に行う処理
        canCountDown = false;
        minutes = 0;
        seconds = 0;
        //centerText.text = "FINISH!";
        StopManager stopM = GameObject.FindFirstObjectByType<StopManager>();
        stopM.EndGame();
        Debug.Log("カウントダウン停止。一度だけログが出力されていたらOK");

        //待った後に行う処理
        yield return new WaitForSeconds(3);
        //resultPanel.SetActive(true);
        SceneManager.LoadScene("ResultSceneClassroom");
    }
}
