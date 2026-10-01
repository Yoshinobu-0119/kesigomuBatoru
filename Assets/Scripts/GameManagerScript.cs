using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        //--------------------------------------------------------------
        //Important
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        DontDestroyOnLoad(gameObject);
    }
    //--------------------------------------------------------------

}

