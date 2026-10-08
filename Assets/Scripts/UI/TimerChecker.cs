using UnityEngine;

public class TimerChecker : MonoBehaviour
{
    private TimerScript timerScript;
    public int timeFinale;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        timerScript = GameObject.FindAnyObjectByType<TimerScript>();
    }
    // Update is called once per frame
    void Update()
    {
        if (timeFinale >= timerScript.time)
        {
            anim.SetBool("MinuteCheck", true);
        }
    }
}
