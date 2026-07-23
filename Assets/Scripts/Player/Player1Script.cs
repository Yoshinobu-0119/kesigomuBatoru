using System.Collections;
using UnityEngine;

public class Player1Script : MonoBehaviour
{
    int PlayerID = 1;
    private Rigidbody rb;
    private PoworBar po;

    //飛ばす強さ
    public float charge;
    private float strength = 0;
    private float max = 25;

    private Vector3 moveDirection = Vector3.zero;

    //【追加】アイテム効果用の変数
    private float speedMultiplier = 1f; // 速度倍率（通常は1倍）
    private bool isReversed = false;     // 操作反転フラグ


    public enum PlayerState
    {
        None,
        ScoreUP,
        SpeedUP,
        Debuff
    }

    public PlayerState state = PlayerState.None;
    void Start()
    {
        Application.targetFrameRate = 60;
        //物理コンポーネントの取得
        rb = GetComponent<Rigidbody>();
        po = GetComponent<PoworBar>();

    }

    void Update()
    {

        //左スティック（または方向キー）で方向を定める
        float horizontal = Input.GetAxis("Horizontal" + PlayerID);
        float vertical = Input.GetAxis("Vertical" + PlayerID);

        //【追加】操作反転フラグが立っている場合、入力を逆にする
        if (isReversed)
        {
            horizontal = -horizontal;
            vertical = -vertical;
        }


        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            charge += 0.5f;
            if (charge > max)
            {
                charge = 0;
            }
            //移動方向の逆を計算
            moveDirection = -new Vector3(horizontal, 0f, vertical).normalized;

            if (Input.GetButtonDown("Attack" + PlayerID))
            {
                strength = charge;
                MovePowor();
            }
        }
        else
        {
            charge = 0;
        }

        po.powor = charge;
    }

    void MovePowor()
    {
        if (moveDirection != Vector3.zero && rb.linearVelocity.magnitude < 1f )
        {

            // --- 【変更】speedMultiplier を掛け合わせて力を加える ---
            float finalStrength = strength * speedMultiplier;
            rb.AddForce(moveDirection * finalStrength, ForceMode.Impulse);
            //rb.AddForce(moveDirection * strength, ForceMode.Impulse);
        }
    }

    //状態異常の追加について

    //自分の速度を上げる（スピードアップ）
    public void ActivateSpeedBoost(float duration, float multiplier)
    {
        StartCoroutine(SpeedBoostRoutine(duration, multiplier));
    }

    private IEnumerator SpeedBoostRoutine(float duration, float multiplier)
    {
        state = PlayerState.SpeedUP;
        speedMultiplier = multiplier; // 速度を○倍にする
        yield return new WaitForSeconds(duration); // duration秒待つ
        speedMultiplier = 1f; // 元に戻す
        state = PlayerState.None;
        Debug.Log("1pスピードもドル");

    }


    //操作を反転させる（デバフ）
    public void ActivateReverseControls(float duration)
    {
        StartCoroutine(ReverseControlsRoutine(duration));
    }

    private IEnumerator ReverseControlsRoutine(float duration)
    {
        state = PlayerState.Debuff;
        isReversed = true; // 反転フラグをON
        yield return new WaitForSeconds(duration); // duration秒待つ
        isReversed = false; // 元に戻す
        state = PlayerState.None;
        Debug.Log("1p反転戻る");
    }

    //スコアアップState状態にする
    public void ActivateState(float duration)
    {
        StartCoroutine(StateRoutine(duration));
    }
    private IEnumerator StateRoutine(float duration)
    {
        state = PlayerState.ScoreUP;
        yield return new WaitForSeconds(duration); // duration秒待つ
        state = PlayerState.None;
        Debug.Log("1pのスコアアップ終わり");
    }




    //アイテムに当たった際の処理
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ItemBox"))
        {
            if(state == PlayerState.None)
            {
                int randomValue = UnityEngine.Random.Range(0, 3); // 0, 1, 2 のいずれか

                switch (randomValue)
                {
                    case 0:
                        // 自分を5秒間、吹っ飛び速度1.5倍にする
                        ActivateSpeedBoost(8f, 1.5f);
                        Debug.Log("1p吹っ飛ばし力アップ");
                        break;
                    case 1:
                        {
                            Debug.Log("2p相手反転");

                            GameObject pl2 = GameObject.FindGameObjectWithTag("Player2");
                            Player2Script state = pl2.GetComponent<Player2Script>();

                            if (state != null)
                            {
                                state.ActivateReverseControls(5.0f);
                            }
                        }
                        break;
                    case 2:
                        Debug.Log("1pスコアアップ");
                        ActivateState(8.0f);
                        break;

                }
            }
            else
            {
                Debug.Log("アイテム取得中");
            }
        }
    }
}
