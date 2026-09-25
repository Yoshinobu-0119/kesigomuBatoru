using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float speedRotation;
    private float angleDirection;

    // Update is called once per frame
    void Update()
    {
        angleDirection += speedRotation * 100 * Time.deltaTime;

        transform.rotation = Quaternion.Euler(angleDirection, angleDirection, angleDirection);
    }
}
