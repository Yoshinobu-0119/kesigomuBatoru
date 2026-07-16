using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class PoworBar : MonoBehaviour
{
    public Image imgHP;     //—Îƒo[
    public int maxHP;       //Å‘å’l

    public float powor;         //HP
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        powor = 0;     //‰Šú‰»
    }

    void Update()
    {
        imgHP.fillAmount = (float)powor / maxHP;
    }

}
