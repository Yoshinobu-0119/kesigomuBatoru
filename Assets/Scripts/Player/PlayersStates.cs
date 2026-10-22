using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayersStates : MonoBehaviour
{
    [Header("Sprite Display")]
    public Image status1p;
    public Image status2p;

    [Header("Sprites (0:None, 1:ScoreUP, 2:SpeedUP, 3:Debuff, 4:BigUp)")]
    public Sprite[] img;

    [Header("Player References")]
    public Player1Script pl1;
    public Player2Script pl2;

    [Header("Shuffle Settings")]
    [SerializeField] private float shuffleInterval = 0.05f; // シャッフルの切り替え速度（秒）

    [Header("Flash Effect Settings")]
    [SerializeField] private float flashInterval = 0.08f;   // 点滅の切り替え間隔（秒）
    [SerializeField] private int flashCount = 6;            // 点滅させる回数（偶数にすると原色で終わります）
    [SerializeField] private Color p1FlashColor = Color.blue; // 1P獲得時の点滅色
    [SerializeField] private Color p2FlashColor = Color.red;  // 2P獲得時の点滅色

    // 前回の状態保持
    private Player1Script.PlayerState lastState1P;
    private Player2Script.PlayerState lastState2P;

    private Coroutine coroutine1P;
    private Coroutine coroutine2P;

    void Start()
    {
        if (pl1 != null)
        {
            lastState1P = pl1.state;
            Update1PVisual(pl1.state);
        }

        if (pl2 != null)
        {
            lastState2P = pl2.state;
            Update2PVisual(pl2.state);
        }
    }

    void Update()
    {
        // 1Pの状態変化チェック
        if (pl1 != null && pl1.state != lastState1P)
        {
            lastState1P = pl1.state;
            Update1PVisual(pl1.state);
        }

        // 2Pの状態変化チェック
        if (pl2 != null && pl2.state != lastState2P)
        {
            lastState2P = pl2.state;
            Update2PVisual(pl2.state);
        }
    }

    //----------------------------------------------------------------------------
    // 状態変更時のロジック
    //----------------------------------------------------------------------------
    private void Update1PVisual(Player1Script.PlayerState state)
    {
        if (coroutine1P != null) StopCoroutine(coroutine1P);

        if (state == Player1Script.PlayerState.None)
        {
            // 未所持状態：シャッフル開始
            coroutine1P = StartCoroutine(ShuffleRoutine(status1p));
        }
        else
        {
            // アイテム獲得：点滅演出後に確定スプライトを表示
            Sprite targetSprite = GetSpriteByState1P(state);
            coroutine1P = StartCoroutine(FlashRoutine(status1p, targetSprite, p1FlashColor));
        }
    }

    private void Update2PVisual(Player2Script.PlayerState state)
    {
        if (coroutine2P != null) StopCoroutine(coroutine2P);

        if (state == Player2Script.PlayerState.None)
        {
            // 未所持状態：シャッフル開始
            coroutine2P = StartCoroutine(ShuffleRoutine(status2p));
        }
        else
        {
            // アイテム獲得：点滅演出後に確定スプライトを表示
            Sprite targetSprite = GetSpriteByState2P(state);
            coroutine2P = StartCoroutine(FlashRoutine(status2p, targetSprite, p2FlashColor));
        }
    }

    //----------------------------------------------------------------------------
    // アニメーション用コルーチン
    //----------------------------------------------------------------------------
    // ① シャッフル演出（None時に画像をパラパラ切り替える）
    private IEnumerator ShuffleRoutine(Image targetImage)
    {
        if (targetImage == null || img == null || img.Length <= 1) yield break;

        targetImage.color = Color.white; // 通常色に戻す

        int index = 1;
        while (true)
        {
            // 配列の1以降（アイテム画像）を順番に切り替え
            targetImage.sprite = img[index];
            index = (index % (img.Length - 1)) + 1; // 1〜(Length-1)をループ

            yield return new WaitForSeconds(shuffleInterval);
        }
    }

    // ② アイテムゲット時の点滅演出（原色 ⇔ 指定色）
    private IEnumerator FlashRoutine(Image targetImage, Sprite targetSprite, Color flashColor)
    {
        if (targetImage == null) yield break;

        // 対象のスプライトをセット
        targetImage.sprite = targetSprite;
        Color defaultColor = Color.white;

        // 指定回数ぶん原色と点滅色を交互に切り替え
        for (int i = 0; i < flashCount; i++)
        {
            targetImage.color = (i % 2 == 0) ? flashColor : defaultColor;
            yield return new WaitForSeconds(flashInterval);
        }

        // 点滅が終わったら原色（白）で固定
        targetImage.color = defaultColor;
    }

    //----------------------------------------------------------------------------
    // スプライト取得メソッド
    //----------------------------------------------------------------------------
    private Sprite GetSpriteByState1P(Player1Script.PlayerState state)
    {
        int index = (int)state;
        return (index >= 0 && index < img.Length) ? img[index] : img[0];
    }

    private Sprite GetSpriteByState2P(Player2Script.PlayerState state)
    {
        int index = (int)state;
        return (index >= 0 && index < img.Length) ? img[index] : img[0];
    }
}