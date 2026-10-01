using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class MenuManager : MonoBehaviour
{
    Vector3[] StartPos;
    public Vector3 EndPos;
    

    public static string revengeStage;

    public Renderer[] buttons;
    public string[] sceneNames;

    int index = 0;
    bool stick = false;
    private Animator anim;
    bool Selecting = false;

    void Start()
    {
        StartPos = new Vector3[buttons.Length];
        for (int i = 0; i < buttons.Length; i++) {
            StartPos[i]=buttons[i].transform.position;
        }
        UpdateButton();

    }

    void Update()
    {
        if (Selecting)
            return;
        float v = Input.GetAxisRaw("Vertical1");
        if (!stick)
        {
            if (v > 0.5f)
            {
                index--;
                if (index < 0)
                    index = buttons.Length - 1;


                UpdateButton();
                stick = true;
            }
            else if (v < -0.5f)
            {
                index++;
                if (index > buttons.Length - 1)
                    index = 0;


                UpdateButton();
                stick = true;
            }





        }

        if (Mathf.Abs(v) < 0.2f)
            stick = false;

        if (Input.GetButtonDown("Attack1"))
        {
            if (index < 5)
            {

                buttons[index].material.SetColor("_BaseColor", Color.red * 5f);
                buttons[index].material.SetColor("_EmissionColor", Color.red * 5f);

                MenuManager.revengeStage = sceneNames[index];
                SceneManager.LoadScene(sceneNames[index]);
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

        for (int i = 0; i < buttons.Length; i++) {
            Vector3 target;
            if (i == index) { target = EndPos; }
            else { target = StartPos[i]; }
            buttons[i].transform.position = Vector3.MoveTowards(buttons[i].transform.position,target,200f * Time.deltaTime);
        
        
        }



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

        // 
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].material.SetColor("_EmissionColor", Color.black);
        }

        // 
        buttons[R].material.EnableKeyword("_EMISSION");
        buttons[R].material.SetColor("_BaseColor", Color.red * 5f);
        buttons[R].material.SetColor("_EmissionColor", Color.red * 5f);

        yield return new WaitForSeconds(1.2f);

        MenuManager.revengeStage = sceneNames[R];
        SceneManager.LoadScene(sceneNames[R]);
    }

}