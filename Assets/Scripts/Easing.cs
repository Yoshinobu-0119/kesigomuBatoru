using UnityEngine;

public static class Ease
{
    public static float Linear(float t) => t;

    public static float InQuad(float t) => t * t;
    public static float OutQuad(float t) => 1 - (1 - t) * (1 - t);
    public static float InOutQuad(float t)
        => t < 0.5f ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;

    public static float InCubic(float t) => t * t * t;
    public static float OutCubic(float t) => 1 - Mathf.Pow(1 - t, 3);
    public static float InOutCubic(float t)
        => t < 0.5f ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;

    public static float InQuart(float t) => t * t * t * t;
    public static float OutQuart(float t) => 1 - Mathf.Pow(1 - t, 4);
    public static float InOutQuart(float t)
        => t < 0.5f ? 8 * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 4) / 2;

    public static float InQuint(float t) => t * t * t * t * t;
    public static float OutQuint(float t) => 1 - Mathf.Pow(1 - t, 5);
    public static float InOutQuint(float t)
        => t < 0.5f ? 16 * t * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 5) / 2;

    public static float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1;
        return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
    }

    public static float EaseInBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1;
        return c3 * t * t * t - c1 * t * t;
    }

    public static float EaseInOutBack(float t)
    {
        float c1 = 1.70158f;
        float c2 = c1 * 1.525f;

        return t < 0.5f
            ? (Mathf.Pow(2 * t, 2) * ((c2 + 1) * 2 * t - c2)) / 2
            : (Mathf.Pow(2 * t - 2, 2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2) / 2;
    }
}
