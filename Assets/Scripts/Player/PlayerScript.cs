using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;

    //飛ばす強さ
    public float strength;

    private Vector3 moveDirection = Vector3.zero;

    public enum State { 
        Stop,
        Move,
        Lose,
    }

    State state;

    void Start()
    {
        //物理コンポーネントの取得
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 1. 左スティック（または方向キー）で方向を定める
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
    }
}
