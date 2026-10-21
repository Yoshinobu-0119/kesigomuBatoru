using UnityEngine;

public class KillTimer : MonoBehaviour
{
    public float killTime;
    void Start()
    {
        Destroy(gameObject,killTime);
    }
}
