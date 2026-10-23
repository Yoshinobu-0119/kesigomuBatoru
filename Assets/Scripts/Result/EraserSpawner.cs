using System.Collections;
using UnityEngine;

public class EraserSpawner : MonoBehaviour
{
    [SerializeField] GameObject redEraser;
    [SerializeField] GameObject blueEraser;

    [SerializeField] Vector3 spawnRangeMin;
    [SerializeField] Vector3 spawnRangeMax;

    // 出現させる回数（インスペクターから指定可能）
    [SerializeField] private int spawnCount = 10;

    private void Start()
    {
        spawnRangeMin = this.transform.position - this.transform.localScale / 2;
        spawnRangeMax = this.transform.position + this.transform.localScale / 2;
    }

    public void SpawnErasers(int score1p, int score2p, float sec)
    {
        if (score1p > score2p)
        {
            // 1Pスコア優勢：redをsec間隔でspawnCount回生成
            StartCoroutine(SpawnRoutine(redEraser, spawnCount, sec));
        }
        else if (score1p < score2p)
        {
            // 2Pスコア優勢：blueをsec間隔でspawnCount回生成
            StartCoroutine(SpawnRoutine(blueEraser, spawnCount, sec));
        }
        else
        {
            // 同点：redとblueを半数ずつsec間隔で生成
            int halfCount = Mathf.Max(1, spawnCount / 2);
            StartCoroutine(SpawnRoutine(redEraser, halfCount, sec));
            StartCoroutine(SpawnRoutine(blueEraser, halfCount, sec));
        }
    }

    // 指定された間隔（sec）で1つずつ生成するコルーチン
    private IEnumerator SpawnRoutine(GameObject prefab, int count, float sec)
    {
        if (prefab == null) yield break;

        for (int i = 0; i < count; i++)
        {
            // Cube内の範囲でランダムな座標を算出
            Vector3 randomPos = new Vector3(
                Random.Range(spawnRangeMin.x, spawnRangeMax.x),
                Random.Range(spawnRangeMin.y, spawnRangeMax.y),
                Random.Range(spawnRangeMin.z, spawnRangeMax.z)
            );

            // X, Y, Z軸すべての回転を0〜360度の間でランダムに決定
            Quaternion randomRotation = Quaternion.Euler(
                Random.Range(0f, 360f),
                Random.Range(0f, 360f),
                Random.Range(0f, 360f)
            );

            // ランダムな位置とランダムな角度で生成
            Instantiate(prefab, randomPos, randomRotation);

            // sec秒待機
            yield return new WaitForSeconds(sec);
        }
    }
}