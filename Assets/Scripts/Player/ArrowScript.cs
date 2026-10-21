using UnityEngine;
using UnityEngine.UI;

public class ArrowScript : MonoBehaviour
{
    public RectTransform arrowUI;
    public int ID;

    public Rigidbody playerRigidbody;
    void Start()
    {
        if(arrowUI != null)arrowUI.gameObject.SetActive(false);
    }
    void Update()
    {
        if (StopManager.CurrentState != StopManager.GameState.Gameplay)
        {
            return;
        }

        if (playerRigidbody != null && playerRigidbody.linearVelocity.magnitude > 1f)
        {
            arrowUI.gameObject.SetActive(false);
            return;
        }

        float keyboardX= 0;
        float keyboardY = 0;
        float gamepadX = 0;
        float gamepadY = 0;
        if (ID == 1)
        {

            keyboardX += Input.GetKey(KeyCode.D) ? 1 : 0;
            keyboardX += Input.GetKey(KeyCode.A) ? -1 : 0;
            gamepadX = Input.GetAxis("Horizontal1");


            keyboardY += Input.GetKey(KeyCode.W) ? 1 : 0;
            keyboardY += Input.GetKey(KeyCode.S) ? -1 : 0;
            gamepadY = Input.GetAxis("Vertical1");
        }
        else if (ID == 2)
        {
            keyboardX += Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
            keyboardX += Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
            gamepadX = Input.GetAxis("Horizontal2");

            keyboardY += Input.GetKey(KeyCode.UpArrow) ? 1 : 0;
            keyboardY += Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
            gamepadY = Input.GetAxis("Vertical2");
        }

        //スティックの入力を取得
        float moveHorizontal = keyboardX != 0 ? keyboardX : gamepadX;
        float moveVertical = keyboardY != 0 ? keyboardY : gamepadY;



        //入力ベクトルを作成
        Vector2 inputDirection = -new Vector2(moveHorizontal,moveVertical);

        if (inputDirection.magnitude > 0.1f /*&& move*/)
        {
            arrowUI.gameObject.SetActive(true);

            //入力ベクトルから角度（ラジアン）を計算し、度数法（Degree）に変換
            float angle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;

            //UIのZ軸の回転に角度を適用する
            arrowUI.rotation = Quaternion.Euler(90, 0, angle);
        }
        else
        {
            // スティックが離されたら非表示
            arrowUI.gameObject.SetActive(false);

        }
    }

    public void MoveArrow()
    {

    }
}
