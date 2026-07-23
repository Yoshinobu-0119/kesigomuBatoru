using UnityEngine;
using UnityEngine.SceneManagement;
public class Resultmanager : MonoBehaviour
{
    

    public GameObject PausePanel;
    public GameObject revenge;
    public GameObject stage;
    public GameObject title;

    
    public string stageScene;
    public string titleScene;



    bool stick = false;

    int index = 0;

    void Start()
    {
        Time.timeScale = 1f;
        UpdateButton();
        
    }

    void Update()
    {
        

        float h = Input.GetAxisRaw("Horizontal1");

        if (!stick)
        {
            if (h > 0.5f)
            {
                index ++;
                if (index > 2)
                    index = 0;
                UpdateButton();
                stick = true;
            }
            else if (h < -0.5f)
            {
                index --;
                if (index < 0)
                    index = 2;
                UpdateButton();
                stick = true;
            }
        }

        if (Mathf.Abs(h) < 0.2f)
            stick = false;

        if (Input.GetButtonDown("Attack1"))
        {
            if (index == 0)
                SceneManager.LoadScene(MenuManager.revengeStage);
            else if (index == 1)
                SceneManager.LoadScene(stageScene);
            else
                SceneManager.LoadScene(titleScene);
        }



    }


   
    void UpdateButton()
    {
        if (index == 0)
        {
            revenge.transform.localScale = Vector3.one * 1.15f;
            stage.transform.localScale = Vector3.one;
            title.transform.localScale = Vector3.one;
        }
        else if(index == 1)
        {
            revenge.transform.localScale = Vector3.one;
            stage.transform.localScale = Vector3.one * 1.15f;
            title.transform.localScale = Vector3.one;
        }
        else
        {
            revenge.transform.localScale = Vector3.one;
            stage.transform.localScale = Vector3.one;
            title.transform.localScale = Vector3.one * 1.15f;
        }
    }
}
