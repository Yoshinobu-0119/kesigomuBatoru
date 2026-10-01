using UnityEngine;

public class PlaceSurface : MonoBehaviour
{
    [SerializeField]
    private PlaceType[] acceptedTypes;

    public bool CanPlace(PlaceType type)
    {
        foreach (var acceptedType in acceptedTypes)
        {
            if (acceptedType == type)
                return true;
        }

        return false;
    }
}