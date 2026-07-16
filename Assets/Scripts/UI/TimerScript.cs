using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI frontText;
    [SerializeField] private TextMeshProUGUI backText;

    [Header("Minutes(•ª)")]
    public int minutes;

    [Header("Seconds(•b)")]
    public int seconds;

    private float timer;

    // Update is called once per frame
    void Update()
    {
        //count down
        //in time
        if (minutes >= 0 && seconds >= 0)
        {
            timer += Time.deltaTime;
        }
         
        //times up
        else if (minutes <= 0 && seconds <= 0)
        {
            minutes = 0;
            seconds = 0;
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
}
