using UnityEngine;

public class PatternMovement : MonoBehaviour
{
    public float speedX;

    private RectTransform rectT;

    public float endPostionX;
    public float startPositionX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectT = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rectT.position = rectT.position + new Vector3(speedX * Time.fixedDeltaTime, 0, 0);

        if(rectT.position.x >= endPostionX)
        {
            rectT.position = rectT.position + new Vector3(startPositionX, 0, 0);
        }
    }
}
