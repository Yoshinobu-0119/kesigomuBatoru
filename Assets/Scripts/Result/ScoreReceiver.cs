using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
public class ScoreReceiver : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem particle;
    [SerializeField] FadeController fadeController;
    [SerializeField] private int score1p;
    [SerializeField] private int score2p;
    private int scoreTotal;
    bool canRestart;

    [Header("UI")]
    [SerializeField] private Image bar1p;
    [SerializeField] private Image bar2p;
    [SerializeField] private TextMeshProUGUI counter1p;
    [SerializeField] private TextMeshProUGUI counter2p;
    [SerializeField] private GameObject CanvasResult;

    [Header("Animation Settings")]
    [Tooltip("最初のダミー伸びの目標割合（0.1 = 10%）")]
    [SerializeField] private float initialFillAmount = 0.1f;

    [Tooltip("最初のダミー上昇にかける時間（秒）")]
    [SerializeField] private float initialDuration = 0.5f;

    [Tooltip("本番アニメーション開始までの待機時間（秒）")]
    [SerializeField] private float waitTime = 1.0f;

    [Tooltip("本番のスコアまで伸ばす時間（秒）")]
    [SerializeField] private float mainDuration = 1.5f;

    private void Start()
    {
        CanvasResult.SetActive(false);
        canRestart = false;
        fadeController.FadeIn(0.5f, EaseType.Linear);
        // 1. スコアデータの取得
        FetchScore();

        // 2. UIの初期化
        if (bar1p != null) bar1p.fillAmount = 0f;
        if (bar2p != null) bar2p.fillAmount = 0f;
        if (counter1p != null) counter1p.text = "0";
        if (counter2p != null) counter2p.text = "0";

        // 3. アニメーション演出開始
        StartCoroutine(AnimateScoreRoutine());
    }

    private void Update()
    {
        if (canRestart && Input.GetButtonDown("Attack1") || canRestart && Input.GetButtonDown("Attack2"))
        {
            CanvasResult.SetActive(true);
        }
    }

    private void FetchScore()
    {
        if (ScoreManager.Instance == null) score1p = 100;
        if (ScoreManager.Instance == null) score2p = 100;
        if (ScoreManager.Instance == null) scoreTotal = 200;
            score1p = ScoreManager.Instance.player1Score;
            score2p = ScoreManager.Instance.player2Score;
        scoreTotal = score1p + score2p;
    }

    private IEnumerator AnimateScoreRoutine()
    {
        // 念のためアニメーション開始時にも最新値を取り直す
        FetchScore();
        yield return new WaitForSeconds(waitTime);
        // --------------------------------------------------
        // フェーズ1: 最初にバーを両方0.1程度まで勢いよく伸ばす
        // --------------------------------------------------
        float elapsed = 0f;

        int initialDummy1p = Mathf.RoundToInt(score1p * initialFillAmount);
        int initialDummy2p = Mathf.RoundToInt(score2p * initialFillAmount);

        while (elapsed < initialDuration)
        {
            elapsed += Time.deltaTime;

            float rawT = Mathf.Clamp01(elapsed / initialDuration);
            float t = Ease.OutQuart(rawT);

            float currentFill = Mathf.Lerp(0f, initialFillAmount, t);
            if (bar1p != null) bar1p.fillAmount = currentFill;
            if (bar2p != null) bar2p.fillAmount = currentFill;

            if (counter1p != null) counter1p.text = Mathf.RoundToInt(Mathf.Lerp(0, initialDummy1p, t)).ToString();
            if (counter2p != null) counter2p.text = Mathf.RoundToInt(Mathf.Lerp(0, initialDummy2p, t)).ToString();

            yield return null;
        }

        if (bar1p != null) bar1p.fillAmount = initialFillAmount;
        if (bar2p != null) bar2p.fillAmount = initialFillAmount;
        if (counter1p != null) counter1p.text = initialDummy1p.ToString();
        if (counter2p != null) counter2p.text = initialDummy2p.ToString();

        // --------------------------------------------------
        // フェーズ2: 指定秒数待機
        // --------------------------------------------------
        yield return new WaitForSeconds(waitTime);

        // --------------------------------------------------
        // フェーズ3: スコアに対応する割合・数値まで勢いよく伸ばす
        // --------------------------------------------------
        elapsed = 0f;

        // 本番の目標値設定前に、もう一度スコアを最終確認
        FetchScore();

        float targetFill1p = scoreTotal > 0 ? (float)score1p / scoreTotal : 0f;
        float targetFill2p = scoreTotal > 0 ? (float)score2p / scoreTotal : 0f;

        while (elapsed < mainDuration)
        {
            elapsed += Time.deltaTime;

            float rawT = Mathf.Clamp01(elapsed / mainDuration);
            float t = Ease.OutCubic(rawT);

            if (bar1p != null) bar1p.fillAmount = Mathf.Lerp(initialFillAmount, targetFill1p, t);
            if (bar2p != null) bar2p.fillAmount = Mathf.Lerp(initialFillAmount, targetFill2p, t);

            if (counter1p != null) counter1p.text = Mathf.RoundToInt(Mathf.Lerp(initialDummy1p, score1p, t)).ToString();
            if (counter2p != null) counter2p.text = Mathf.RoundToInt(Mathf.Lerp(initialDummy2p, score2p, t)).ToString();

            yield return null;
        }

        // 最終値を確定
        if (bar1p != null) bar1p.fillAmount = targetFill1p;
        if (bar2p != null) bar2p.fillAmount = targetFill2p;
        if (counter1p != null) counter1p.text = score1p.ToString();
        if (counter2p != null) counter2p.text = score2p.ToString();

        PlayAnimTrigger("StartAnim");
        yield return new WaitForSeconds(1f);
        particle.Play();
        canRestart = true;
    }

    public void PlayAnimTrigger(string AnimName)
    {
        animator.SetTrigger(AnimName);
    }
}