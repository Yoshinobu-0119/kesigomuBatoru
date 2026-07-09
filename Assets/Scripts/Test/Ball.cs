using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    private Rigidbody physics = null;

    [SerializeField]
    private LineRenderer direction = null;

    private const float MaxMagnitude = 2f;

    private Vector3 currentForce = Vector3.zero;

    private Camera mainCamera = null;

    private float cameraDistance = 10f; // ← ここが重要（カメラからの距離）

    private Vector3 dragStart = Vector3.zero;

    public void Awake()
    {
        physics = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

        // カメラが上から見ている前提
        // カメラの高さ（Z）とボールの高さ（Z）の差を使う
        cameraDistance = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
    }

    /// <summary>
    /// マウス座標をワールド座標に変換（トップダウン用）
    /// </summary>
    private Vector3 GetMousePosition()
    {
        var pos = Input.mousePosition;

        // カメラからの距離を補完
        pos.z = cameraDistance;

        var world = mainCamera.ScreenToWorldPoint(pos);

        // XY 平面に固定
        world.z = 0;

        return world;
    }

    public void OnMouseDown()
    {
        dragStart = GetMousePosition();

        direction.enabled = true;
        direction.SetPosition(0, physics.position);
        direction.SetPosition(1, physics.position);
    }

    public void OnMouseDrag()
    {
        var pos = GetMousePosition();

        currentForce = pos - dragStart;

        // 最大力制限
        if (currentForce.magnitude > MaxMagnitude)
        {
            currentForce = currentForce.normalized * MaxMagnitude;
        }

        direction.SetPosition(0, physics.position);
        direction.SetPosition(1, physics.position + currentForce);
    }

    public void OnMouseUp()
    {
        direction.enabled = false;

        // XY 平面で力を加える
        Flip(currentForce * 6f);
    }

    public void Flip(Vector3 force)
    {
        physics.AddForce(force, ForceMode.Impulse);
    }
}
