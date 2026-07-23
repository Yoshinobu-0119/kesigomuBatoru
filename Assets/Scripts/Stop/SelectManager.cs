using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{

    public static string revengeStage;

    public Renderer[] buttons;
    public string[] sceneNames;

    int index = 0;
    bool stick = false;

    void Start()
    {
        UpdateButton();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal1");

        if (!stick)
        {
            if (h > 0.5f)
            {
                index++;

                if (index >= buttons.Length)
                    index = 0;

                UpdateButton();
                stick = true;
            }
            else if (h < -0.5f)
            {
                index--;

                if (index < 0)
                    index = buttons.Length - 1;

                UpdateButton();
                stick = true;
            }
        }

        if (Mathf.Abs(h) < 0.2f)
            stick = false;

        if (Input.GetButtonDown("Attack1"))
        {
            MenuManager.revengeStage = sceneNames[index];
            SceneManager.LoadScene(sceneNames[index]);
        }
    }

    void UpdateButton()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == index)
            {
                buttons[i].material.EnableKeyword("_EMISSION");
                buttons[i].material.SetColor("_EmissionColor", Color.yellow * 5f);
            }
            else
            {
                buttons[i].material.SetColor("_EmissionColor", Color.black);
            }
        }
    }
}