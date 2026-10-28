using System.Collections;
using UnityEngine;

public class EraserSpawner : MonoBehaviour
{
    [SerializeField] private GameObject redEraser;
    [SerializeField] private GameObject blueEraser;

    // 大きい消しゴム用プレハブ（未設定の場合は通常の消しゴムを拡大して生成します）
    [SerializeField] private GameObject bigRedEraser;
    [SerializeField] private GameObject bigBlueEraser;

    // ターゲットの参照
    [SerializeField] private Transform targetR;
    [SerializeField] private Transform targetB;

    // ターゲットの上空何メートルの位置から落とすか
    [SerializeField] private float dropHeight = 5.0f;

    [SerializeField] private Vector3 spawnRangeMin;
    [SerializeField] private Vector3 spawnRangeMax;

    // 出現させる回数（インスペクターから指定可能）
    [SerializeField] private int spawnCount = 10;

    private void Start()
    {
        spawnRangeMin = this.transform.position - this.transform.localScale / 2;
        spawnRangeMax = this.transform.position + this.transform.localScale / 2;
    }

    public void SpawnErasers(int score1p, int score2p, float sec)
    {
        // 全体の生成シーケンスを開始
        StartCoroutine(SpawnSequence(score1p, score2p, sec));
    }

    // デカ消しゴム落下から出現処理までの一連のフローを管理するコルーチン
    private IEnumerator SpawnSequence(int score1p, int score2p, float sec)
    {
        if (score1p > score2p)
        {
            // 1. 先に相手(2P/Blue)のTargetBの上空から大きい赤消しゴムを落とす
            GameObject eraserPrefab = bigRedEraser != null ? bigRedEraser : redEraser;
            DropBigEraser(eraserPrefab, targetB);

            // 2. その後、redをsec間隔で指定回数ランダム生成する
            yield return StartCoroutine(SpawnRoutine(redEraser, spawnCount, sec));
        }
        else if (score1p < score2p)
        {
            // 1. 先に相手(1P/Red)のTargetRの上空から大きい青消しゴムを落とす
            GameObject eraserPrefab = bigBlueEraser != null ? bigBlueEraser : blueEraser;
            DropBigEraser(eraserPrefab, targetR);

            // 2. その後、blueをsec間隔で指定回数ランダム生成する
            yield return StartCoroutine(SpawnRoutine(blueEraser, spawnCount, sec));
        }
        else
        {
            // 同点：redとblueを半数ずつ並列で生成（デカ消しゴムは落とさない）
            int halfCount = Mathf.Max(1, spawnCount / 2);
            Coroutine coroutineR = StartCoroutine(SpawnRoutine(redEraser, halfCount, sec));
            Coroutine coroutineB = StartCoroutine(SpawnRoutine(blueEraser, halfCount, sec));

            yield return coroutineR;
            yield return coroutineB;
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

    // ターゲットの上空から大きい消しゴムを落とす処理
    private void DropBigEraser(GameObject prefab, Transform target)
    {
        if (prefab == null || target == null) return;

        // ターゲットの位置に高さオフセット（dropHeight）を加算した位置
        Vector3 dropPosition = target.position + Vector3.up * dropHeight;

        Quaternion randomRotation = Quaternion.Euler(
            Random.Range(0f, 360f),
            Random.Range(0f, 360f),
            Random.Range(0f, 360f)
        );

        GameObject spawnedEraser = Instantiate(prefab, dropPosition, randomRotation);

        // 専用の大きい消しゴムプレハブがセットされていない場合は、通常の消しゴムを2倍サイズにして生成
        if (prefab == redEraser || prefab == blueEraser)
        {
            spawnedEraser.transform.localScale *= 2.0f;
        }
    }
}