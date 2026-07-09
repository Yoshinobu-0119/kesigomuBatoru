using UnityEngine;

public class Player2Script : MonoBehaviour
{
    private int PlayerID = 2;
    private Rigidbody rb;

    //飛ばす強さ
    public float charge;
    private float strength = 0;
    private float max = 25;

    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        Application.targetFrameRate = 60;
        //物理コンポーネントの取得
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
        //左スティック（または方向キー）で方向を定める
        float horizontal = Input.GetAxis("Horizontal" + PlayerID);
        float vertical = Input.GetAxis("Vertical" + PlayerID);


        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            charge += 0.5f;
            if (charge > max)
            {
                charge = 0;
            }
            //移動方向の逆を計算
            moveDirection = -new Vector3(horizontal, 0f, vertical).normalized;

            if (Input.GetButtonDown("Attack" + PlayerID) && PlayerID == 2)
            {
                strength = charge;
                MovePowor();
            }
        }

    }

    void MovePowor()
    {
        if (moveDirection != Vector3.zero && rb.linearVelocity.magnitude < 1f)
        {
            rb.AddForce(moveDirection * strength, ForceMode.Impulse);
        }
    }
}
