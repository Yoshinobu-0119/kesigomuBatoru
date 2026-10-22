using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class MenuManager : MonoBehaviour
{
    Vector3[] StartPos;
    public Vector3 EndPos;

    public AudioSource audioSource;
    public AudioClip se_select;
    public AudioClip se_push;

    public static string revengeStage;

    public Renderer[] buttons;
    public string[] sceneNames;

    public StagePreview stagePreview;


    int index = 0;
    bool stick = false;
    private Animator anim;
    bool Selecting = false;

    private bool isChanging = false;

    void Start()
    {
        Selecting = true;
        StartPos = new Vector3[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            StartPos[i] = buttons[i].transform.position;
        }
        UpdateButton();
        StartCoroutine(ShowPreviewAfterIntro());
        
    }

    void Update()
    {
        if (Selecting)
            return;
        float v = Input.GetAxisRaw("Vertical1") != 0 ? Input.GetAxisRaw("Vertical1") : Input.GetAxisRaw("Vertical2");
        if (!stick)
        {
            if (v > 0.5f)
            {
                index--;
                if (index < 0)
                    index = buttons.Length - 1;


                UpdateButton();

                // 
                stagePreview.ChangeStage(index, true);
                audioSource.PlayOneShot(se_select,0.5f);
                stick = true;
            }
            else if (v < -0.5f)
            {
                index++;
                if (index > buttons.Length - 1)
                    index = 0;


                UpdateButton();
                //
                stagePreview.ChangeStage(index, false);
                audioSource.PlayOneShot(se_select,0.5f);
                stick = true;
            }





        }

        if (Mathf.Abs(v) < 0.2f)
            stick = false;

        if (!isChanging &&
        (Input.GetButtonDown("Attack1") ||
        Input.GetButtonDown("Attack2")))
        {
            if (index < 5)
            {

                buttons[index].material.SetColor("_BaseColor", Color.red * 5f);
                buttons[index].material.SetColor("_EmissionColor", Color.red * 5f);

                StartCoroutine(ChangeScene());

               /*
                MenuManager.revengeStage = sceneNames[index];
                SceneManager.LoadScene(sceneNames[index]);*/
            }
            if (index == 5)
            {
                
                
                StartCoroutine(RandomStage());
                

                /*int R = Random.Range(0, 5);
                anim = GetComponent<Animator>();
                anim.Play("S1");*/

                /* MenuManager.revengeStage = sceneNames[R];
                 SceneManager.LoadScene(sceneNames[R]);*/
            }
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            Vector3 target;
            if (i == index) { target = EndPos; }
            else { target = StartPos[i]; }
            buttons[i].transform.position = Vector3.MoveTowards(buttons[i].transform.position, target, 200f * Time.deltaTime);


        }



    }


    private IEnumerator ChangeScene() {
    isChanging = true;
        audioSource.PlayOneShot(se_push);
        yield return new WaitForSecondsRealtime(se_push.length);
        MenuManager.revengeStage = sceneNames[index];
        SceneManager.LoadScene(sceneNames[index]);
    }


    /* public void ClickRandomButton()
     {

             index = Random.Range(0, 5);
             MenuManager.revengeStage = sceneNames[index];
             SceneManager.LoadScene(sceneNames[index]);




     }*/

    void UpdateButton()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == index)
            {



                buttons[i].material.EnableKeyword("_EMISSION");
                buttons[i].material.SetColor("_EmissionColor", Color.yellow * 5f);
            }
            else
            {
                buttons[i].transform.position = StartPos[i];
                buttons[i].material.SetColor("_EmissionColor", Color.black);

            }
        }
    }

    IEnumerator RandomStage()
    {
        Selecting = true;
        int R = Random.Range(0, 5);

        stagePreview.ChangeStage(R, false);

        // 
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].material.SetColor("_EmissionColor", Color.black);
        }

        // 
        buttons[R].material.EnableKeyword("_EMISSION");
        buttons[R].material.SetColor("_BaseColor", Color.red * 5f);
        buttons[R].material.SetColor("_EmissionColor", Color.red * 5f);
        audioSource.PlayOneShot(se_push, 2f);
        yield return new WaitForSeconds(1.2f);

        MenuManager.revengeStage = sceneNames[R];
        SceneManager.LoadScene(sceneNames[R]);
    }





    IEnumerator ShowPreviewAfterIntro()
    {
        // 
        yield return new WaitForSeconds(1f);

        stagePreview.ShowFirstImage();
        Selecting = false;
    }
}