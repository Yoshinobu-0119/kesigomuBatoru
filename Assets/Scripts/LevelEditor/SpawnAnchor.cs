using UnityEngine;

public class SpawnAnchor : MonoBehaviour
{
    [SerializeField]
    private Transform groundPoint;

    public float GetGroundOffset()
    {
        if (groundPoint == null)
            return 0f;

        return -groundPoint.localPosition.y;
    }
}