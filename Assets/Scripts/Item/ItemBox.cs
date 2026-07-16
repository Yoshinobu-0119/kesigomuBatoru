using UnityEngine;
using System;

public class ItemBox : MonoBehaviour
{
    // アイテムが破壊されたときに呼び出されるイベント
    public event Action OnDestroyed;

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーに当たった時の処理
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            //登録されている（ItemManager）に通知する
            OnDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }
}
