using System.Collections;
using UnityEngine;

public class Player1Script : MonoBehaviour
{
    //プレイヤーIDとか
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

    //ScoreManagerから取得
    public ScoreManager scoreManager;

    private float rb_defaultmass;
    Vector3 defaultscale;

    public enum PlayerState
    {
        None,
        ScoreUP,
        SpeedUP,
        Debuff,
        BigUp,
    }

    public PlayerState state = PlayerState.None;

    [Header("Model's specific part")]
    [SerializeField] private MeshRenderer meshRenderer;
    private int modelCover = 0;
    private int modelEraser = 1;

    [Header("Palettes")]
    [SerializeField] private Material normalCoverPalette;
    [SerializeField] private Material normalRubberPalette;
    [SerializeField] private Material scoreUpPalette;
    [SerializeField] private Material speedUpPalette;
    [SerializeField] private Material debuffPalette;
    [SerializeField] private Material bigUpPalette;

    void Start()
    {
        Application.targetFrameRate = 60;
        //物理コンポーネントの取得
        rb = GetComponent<Rigidbody>();
        po = GetComponent<PoworBar>();
        defaultscale = this.transform.localScale;
        rb_defaultmass = rb.mass;
    }

    void Update()
    {
        if (StopManager.CurrentState != StopManager.GameState.Gameplay)
        {
            return;
        }

        //キーボードとゲームパッドの入力を取得
        float keyboardX = 0;
        keyboardX += Input.GetKey(KeyCode.D) ? 1 : 0;
        keyboardX += Input.GetKey(KeyCode.A) ? -1 : 0;
        float gamepadX = Input.GetAxis("Horizontal1");

        float keyboardY = 0;
        keyboardY += Input.GetKey(KeyCode.W) ? 1 : 0;
        keyboardY += Input.GetKey(KeyCode.S) ? -1 : 0;
        float gamepadY = Input.GetAxis("Vertical1");

        //入力を元に移動に使用する数値を設定
        float horizontal = keyboardX != 0 ? keyboardX : gamepadX;
        float vertical = keyboardY != 0 ? keyboardY : gamepadY;

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

            if (Input.GetButtonDown("Attack" + PlayerID) || Input.GetKeyDown(KeyCode.LeftShift))
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
            GetComponent<AudioSource>().Play();

            // --- 【変更】speedMultiplier を掛け合わせて力を加える ---
            float finalStrength = strength * speedMultiplier;
            rb.AddForce(moveDirection * finalStrength, ForceMode.Impulse);
            //rb.AddForce(moveDirection * strength, ForceMode.Impulse);
        }
    }

    private void changePalette(PlayerState playerState)
    {
        Material[] palette = meshRenderer.materials;
        switch (playerState)
        {
            case PlayerState.None:
                palette[modelCover] = normalCoverPalette;
                break;

            case PlayerState.ScoreUP:
                palette[modelCover] = scoreUpPalette;
                break;

            case PlayerState.SpeedUP:
                palette[modelCover] = speedUpPalette;
                break;

            case PlayerState.Debuff:
                palette[modelEraser] = debuffPalette;

                break;

            case PlayerState.BigUp:
                palette[modelCover] = bigUpPalette;
                break;

        }
        meshRenderer.materials = palette;
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
        changePalette(PlayerState.SpeedUP);
        speedMultiplier = multiplier; // 速度を○倍にする
        yield return new WaitForSeconds(duration); // duration秒待つ
        speedMultiplier = 1f; // 元に戻す
        state = PlayerState.None;
        changePalette(PlayerState.None);
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
        changePalette(PlayerState.ScoreUP);
        yield return new WaitForSeconds(duration); // duration秒待つ
        state = PlayerState.None;
        changePalette(PlayerState.None);
        Debug.Log("1pのスコアアップ終わり");
    }

    //スケールアップ
    public void ActivateBig(float duration , float multiplier)
    {
        StartCoroutine(StateRoutineBig(duration, multiplier));
    }
    private IEnumerator StateRoutineBig(float duration, float multiplier)
    {
        state = PlayerState.BigUp;
        changePalette(PlayerState.BigUp);
        this.transform.localScale = transform.localScale * 2.5f;
        rb.mass = 4f;
        speedMultiplier = multiplier; // 速度を○倍にする
        yield return new WaitForSeconds(duration); // duration秒待つ
        this.transform.localScale = defaultscale;
        rb.mass = rb_defaultmass;
        speedMultiplier = 1f; // 元に戻す
        state = PlayerState.None;
        changePalette(PlayerState.None);
        Debug.Log("縮んじゃった");
    }



    //アイテムに当たった際の処理
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ItemBox"))
        {
            if(state == PlayerState.None)
            {
                int maxRandom = (scoreManager.player1Score < scoreManager.player2Score) ? 4 : 3;
                int randomValue = UnityEngine.Random.Range(0, maxRandom); // 0, 1, 2 のいずれか

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
                    case 3:
                        Debug.Log("1pのがBIGになっちゃった");
                        ActivateBig(8.0f, 2.0f);
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
