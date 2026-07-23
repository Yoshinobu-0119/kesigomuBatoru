using UnityEngine;

public enum EaseType
{
    Linear,
    InQuad,
    OutQuad,
    InOutQuad,
    InCubic,
    OutCubic,
    InOutCubic,
    InQuart,
    OutQuart,
    InOutQuart,
    InQuint,
    OutQuint,
    InOutQuint,
    EaseOutBack,
    EaseInBack,
    EaseInOutBack
}

public class CameraPointMarker : MonoBehaviour
{
    public float moveTime = 0.5f;
    public EaseType easeType = EaseType.InOutCubic;
}
