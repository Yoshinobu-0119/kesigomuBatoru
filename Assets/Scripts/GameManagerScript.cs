using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance { get; private set; }

    private Animator transitionObj;
    private bool isTransitioning;

    //animation length
    private float animLength;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else 
        { 
            Destroy(gameObject);
            return;
        }

        transitionObj = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        //--------------------------------------------------------------
        //ëÂêÿ
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            transitionObj.Play("CloseTransition");
        }*/
    }
    //--------------------------------------------------------------

    public void ChangeScene(string targetScene)
    {
        if(!isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(transitionFlow(targetScene));
            isTransitioning = false;
        }
    }

    public void Button_Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    private IEnumerator transitionFlow(string targetScene)
    {
        transitionObj.Play("CloseTransition");
        yield return null;
        animLength = transitionObj.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(animLength);

        SceneManager.LoadScene(targetScene);
        transitionObj.Play("OpenTransition");
    }
}

