using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class MenuManager : MonoBehaviour
{
    Vector3[] StartPos;
    Vector3[] TargetPos;
    Quaternion[] StartRot;
    Quaternion[] TargetRot;

    public Vector3 EndPos;
    public float Endspeed;

    int Posindex = 0;
    public float move = 2f;
    bool first = true;
    int moveFOR = 1;

    public AudioSource audioSource;
    public AudioClip se_select;
    public AudioClip se_push;

    public static string revengeStage;

    public Renderer[] buttons;
    public string[] sceneNames;

    public string[] Titlescene;

    public StagePreview stagePreview;


    int index = 0;
    bool stick = false;
    private Animator anim;
    bool Selecting = false;

    private bool isChanging = false;

    void Start()
    {
        Selecting = true;
        TargetPos = new Vector3[buttons.Length];
        StartPos = new Vector3[buttons.Length];
        TargetRot = new Quaternion[buttons.Length];
        StartRot = new Quaternion[buttons.Length];
        // StartRot = new Quaternion[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            
            StartPos[i] = buttons[i].transform.position;
            TargetPos[i] = StartPos[i];
            StartRot[i] = buttons[i].transform.localRotation;
            TargetRot[i] = StartRot[i];
        }
        UpdateButton();
        StartCoroutine(ShowPreviewAfterIntro());
        
    }

    void Update()
    {
        if (Selecting)
            return;

        float keyboardX = 0;
        keyboardX += Input.GetKey(KeyCode.LeftArrow) ? 1 : 0;
        keyboardX += Input.GetKey(KeyCode.RightArrow) ? -1 : 0;

        float gamepadX = Input.GetAxisRaw("Horizontal1") != 0 ? Input.GetAxisRaw("Horizontal1") : Input.GetAxisRaw("Horizontal1");
        float v = keyboardX != 0 ? keyboardX : gamepadX;

        if (!stick)
        {
            if (v > 0.5f)
            {
               
                index--;
                if (index < 0)
                    index = buttons.Length - 1;
                moveFOR = 1;
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


                moveFOR = -1;
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

                TargetPos[index] = EndPos;

                StartCoroutine(ChangeScene());

               /*
                MenuManager.revengeStage = sceneNames[index];
                SceneManager.LoadScene(sceneNames[index]);*/
            }
            if (index == 5)
            {
                
                
                StartCoroutine(RandomStage());
                

               
            }
        }

        if (!isChanging &&
        (Input.GetButtonDown("Fire1") ||
        Input.GetButtonDown("Fire2")))
        {
            SceneManager.LoadScene(Titlescene[0]);
        }


            for (int i = 0; i < buttons.Length; i++)
        {
            //Vector3 target;

            float speed = 200f;
            if(isChanging && i == index)
            {
                speed = Endspeed;
            }

            buttons[i].transform.position = 
                Vector3.MoveTowards
                (buttons[i].transform.position, TargetPos[i], speed * Time.deltaTime);

            buttons[i].transform.localRotation =
               Quaternion.RotateTowards
               (buttons[i].transform.localRotation, TargetRot[i], 250f * Time.deltaTime);



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

        if (!first)
        {

            if (moveFOR > 0) { Posindex++; }
            else { Posindex--; }

            if (Posindex >= StartPos.Length) { Posindex = 0; }
            if (Posindex < 0) { Posindex = StartPos.Length - 1; }

        }

        for (int i = 0; i < buttons.Length; i++)
        {
            int p = i + Posindex;
            if(p >= buttons.Length) {p-= buttons.Length; }
            TargetPos[i] = StartPos[p];

            if (i == index)
            {



                buttons[i].material.EnableKeyword("_EMISSION");
                buttons[i].material.SetColor("_EmissionColor", Color.yellow * 5f);
                buttons[i].transform.localScale = new Vector3(20f, 20f, 20f);
                TargetRot[i] = StartRot[i]*Quaternion.Euler(0f, 0f, 0f);

            }
            else
            {
                //buttons[i].transform.position = StartPos[i];
                buttons[i].material.SetColor("_EmissionColor", Color.black);
                buttons[i].transform.localScale = new Vector3(13.73f, 13.73f, 13.73f);
                TargetRot[i] = StartRot[i];
            }
           

        }
    first = false;
    
    }



    IEnumerator RandomStage()
    {
        isChanging = true;
        int R = Random.Range(0, 5);
        buttons[index].material.SetColor("_BaseColor", Color.red * 5f);
        buttons[index].material.SetColor("_EmissionColor", Color.red * 5f);
        TargetPos[index] = EndPos;
        stagePreview.ChangeStage(R, false);

        // 
        /*for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].material.SetColor("_EmissionColor", Color.black);
        }

        // 
        buttons[R].material.EnableKeyword("_EMISSION");
        buttons[R].material.SetColor("_BaseColor", Color.red * 5f);
        buttons[R].material.SetColor("_EmissionColor", Color.red * 5f);*/
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