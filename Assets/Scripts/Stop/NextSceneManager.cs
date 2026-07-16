using UnityEngine;
using UnityEngine.SceneManagement;
public class NextSceneManager : MonoBehaviour
{


    public string nextScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
                SceneManager.LoadScene(nextScene);
        }
    }
}
