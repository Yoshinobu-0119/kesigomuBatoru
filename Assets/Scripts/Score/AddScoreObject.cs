using UnityEngine;

public class AddScoreObject : MonoBehaviour
{
    ScoreManager scoreManager;
    Rigidbody rb;

    [Header("自身がプレイヤーかどうか")]
    public bool isPlayer1p;
    public bool isPlayer2p;

    [Header("復活設定")]
    [SerializeField] bool canRespawn;
    [SerializeField] bool isRandom;
    [SerializeField] Vector3 defaultPos;
    [SerializeField] Quaternion defaultRot;
    [Space(5)]
    [SerializeField] Vector3 randomRangeMin;
    [SerializeField] Vector3 randomRangeMax;    

    [Header("スコア")]
    public int itemScore;
    [SerializeField] bool is1p;

    private void Start()
    {
        //インスタンス化したスコアマネージャーを参照
        scoreManager = ScoreManager.Instance;

        //初期の位置と角度を登録
        defaultPos = transform.position;
        defaultRot = transform.rotation;

        //ここに、プレイヤーのスクリプトがあればisPlayer1p,2pを変更する処理を入れる

        rb = GetComponent<Rigidbody>();
    }


    void Respawn()
    {
        //回転
        transform.rotation = defaultRot;
        //位置
        if (isRandom)
        {
            transform.position = new Vector3(Random.Range(randomRangeMin.x, randomRangeMax.x),
                                             Random.Range(randomRangeMin.y, randomRangeMax.y),
                                             Random.Range(randomRangeMin.z, randomRangeMax.z));
        }
        else
        {
            transform.position = defaultPos;
        }

        //Rigidbodyがあるなら動きを停止
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "void")
        {
            if (this.isPlayer1p) is1p = false;

            if (this.isPlayer2p) is1p = true;

            scoreManager.AddScore(is1p, itemScore);

            if (canRespawn)
            {
                Respawn();
            }
            scoreManager.UpdateScore();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //当たったオブジェクトにAddScoreObjectはある？
        if (collision.gameObject.GetComponent<AddScoreObject>())
        {
            //相手のAddScoreObject
            AddScoreObject other = collision.gameObject.GetComponent<AddScoreObject>();

            //相手がプレイヤーじゃない場合
            if (other.isPlayer1p == false && other.isPlayer2p == false)
            {
                if (this.isPlayer1p) other.is1p = true;

                if (this.isPlayer2p) other.is1p = false;
            }
        }
    }
}
