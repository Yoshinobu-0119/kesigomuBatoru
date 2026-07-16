using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject itemPrefab;

    public float spawntime;

    void Start()
    {
        // ゲーム開始時に最初の1個を生成
        SpawnObject();
    }

    void SpawnObject()
    {
        if (itemPrefab != null)
        {
            // アイテムを生成
            GameObject spawnedObj = Instantiate(itemPrefab, transform.position, transform.rotation);

            // 生成したアイテムからスクリプトを取得
            ItemBox itemBox = spawnedObj.GetComponent<ItemBox>();

            if (itemBox != null)
            {
                itemBox.OnDestroyed += StartSpawnTimer;
            }
        }
    }

    private void StartSpawnTimer()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(spawntime);
        SpawnObject();
    }
}
