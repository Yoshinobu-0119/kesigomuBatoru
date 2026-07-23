using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class CameraPos
{
    public Vector3 pos;
    public Quaternion rot;
    public float moveTime = 0.5f; // 移動時間
    public EaseType easeType;   　// イージングの種類
}

public class IntroCamPlayer : MonoBehaviour
{
    public CameraPos[] camList;
    public FadeController fade;

    public string nextSceneName = "ScoreTestScene";
    public float changeSceneTimer;

    public float startFadeTime = 1f;
    public float endFadeTime = 1f;
    public EaseType fadeEase = EaseType.InOutQuad;

    int index = 0;
    float timer = 0f;

    private void Awake()
    {
        LoadFromScene();
    }

    void Start()
    {
        if (camList.Length > 0)
        {
            // 最初の位置に即座にセット
            transform.position = camList[0].pos;
            transform.rotation = camList[0].rot;
        }

        fade.FadeIn(startFadeTime, fadeEase);
    }

    [ContextMenu("Load Camera Points From Scene")]
    void LoadFromScene()
    {
        var points = FindObjectsOfType<CameraPointMarker>()
            .OrderBy(p => p.transform.GetSiblingIndex()) // Hierarchy順
            .ToArray();

        //ArrayにそれぞれのCameraPointを追加
        camList = points.Select(p => new CameraPos()
        {
            pos = p.transform.position,
            rot = p.transform.rotation,
            moveTime = p.moveTime,
            easeType = p.easeType
        }).ToArray();

        Debug.Log($" {camList.Length} 個のカメラポイントをヒエラルキー順で追加したああああああああ");
    }


    void Update()
    {
        if (index >= camList.Length - 1) return;

        timer += Time.deltaTime;

        CameraPos from = camList[index];
        CameraPos to = camList[index + 1];

        float t = timer / to.moveTime;
        t = Mathf.Clamp01(t);

        float easeT = ApplyEase(to.easeType, t);

        transform.position = Vector3.Lerp(from.pos, to.pos, easeT);
        transform.rotation = Quaternion.Slerp(from.rot, to.rot, easeT);


        // 次のポイントへ
        if (t >= 1f)
        {
            index++;
            timer = 0f;

            if (index >= camList.Length - 1)
            {
                fade.FadeOut(endFadeTime, fadeEase);
                StartCoroutine(LoadScene(nextSceneName,changeSceneTimer));
            }
        }
    }

    IEnumerator LoadScene(string sceneName, float sceneTimer)
    {
        yield return new WaitForSeconds(sceneTimer);
        SceneManager.LoadScene(sceneName);
    }

    //イージングを適用させるやつ
    float ApplyEase(EaseType type, float t)
    {
        return type switch
        {
            EaseType.Linear => Ease.Linear(t),
            EaseType.InQuad => Ease.InQuad(t),
            EaseType.OutQuad => Ease.OutQuad(t),
            EaseType.InOutQuad => Ease.InOutQuad(t),

            EaseType.InCubic => Ease.InCubic(t),
            EaseType.OutCubic => Ease.OutCubic(t),
            EaseType.InOutCubic => Ease.InOutCubic(t),

            EaseType.InQuart => Ease.InQuart(t),
            EaseType.OutQuart => Ease.OutQuart(t),
            EaseType.InOutQuart => Ease.InOutQuart(t),

            EaseType.InQuint => Ease.InQuint(t),
            EaseType.OutQuint => Ease.OutQuint(t),
            EaseType.InOutQuint => Ease.InOutQuint(t),

            EaseType.EaseOutBack => Ease.EaseOutBack(t),
            EaseType.EaseInBack => Ease.EaseInBack(t),
            EaseType.EaseInOutBack => Ease.EaseInOutBack(t),

            _ => t
        };
    }

}
