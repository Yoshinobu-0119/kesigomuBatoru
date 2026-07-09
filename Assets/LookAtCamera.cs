using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [Header("追従するターゲット（キャラクター）")]
    public Transform target;

    [Header("頭上のオフセット位置調整")]
    public Vector3 offset = new Vector3(0, 2.0f, 0);

    private Transform mainCameraTransform;

    void Start()
    {
        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }
    }

    // キャラクターの移動（Updateや物理演算）の後に実行するためLateUpdateを使う
    void LateUpdate()
    {
        if (target == null) return;

        // 位置を追従
        transform.position = target.position + offset;
    }
}

