using UnityEngine;
using UnityEngine.UI;

public class ArrowScript : MonoBehaviour
{
    public RectTransform arrowUI;
    public int ID;
    void Start()
    {
        if(arrowUI != null)arrowUI.gameObject.SetActive(false);
    }
    void Update()
    {
        //スティックの入力を取得
        float moveHorizontal = Input.GetAxisRaw("Horizontal" + ID);
        float moveVertical = Input.GetAxisRaw("Vertical" + ID);

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
