using UnityEngine;

public enum PlaceType
{
    Terrain,
    DeskItem
}

[System.Serializable]
public class PlaceableData
{
    public string prefabId;

    public string displayName;

    public GameObject prefab;

    public PlaceType placeType;
}

public class EditorPlacedObject : MonoBehaviour
{
    [Header("Object Info")]
    public string prefabId;

    public PlaceType placeType;

    [Header("Physics")]
    public bool useRigidBody;

    [Header("Gameplay")]
    public int itemScore = 100;

    public bool canRespawn;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetRigidBodyEnabled(bool enabled)
    {
        useRigidBody = enabled;

        if (rb == null)
            return;

        rb.isKinematic = !enabled;
        rb.useGravity = enabled;
    }
}