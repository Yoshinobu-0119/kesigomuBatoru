using UnityEngine;
using UnityEngine.UI;

public class SceneButton : MonoBehaviour
{
    [SerializeField] private string targetScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        GameManagerScript.instance.ChangeScene(targetScene));
    }
}
