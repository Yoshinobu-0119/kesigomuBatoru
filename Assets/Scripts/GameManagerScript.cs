using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    private Animator anim;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        //--------------------------------------------------------------
        //ëÂêÿ
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        DontDestroyOnLoad(gameObject);
    }
    //--------------------------------------------------------------

}

