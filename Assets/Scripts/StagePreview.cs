
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StagePreview : MonoBehaviour
{
    public Image oldImage;
    public Image newImage;
    public Sprite[] stageImages;

    public float duration = 0.4f;

    Coroutine changing;

    void Start()
    {
        oldImage.type = Image.Type.Simple;
        oldImage.color = Color.white;

        newImage.type = Image.Type.Filled;
        newImage.fillMethod = Image.FillMethod.Horizontal;
        newImage.color = Color.white;

        // 
        oldImage.enabled = false;
        newImage.fillAmount = 0;
    }

    public void ChangeStage(int index, bool upward)
    {
        if (index < 0 || index >= stageImages.Length)
            return;

        if (changing != null)
            StopCoroutine(changing);

        changing = StartCoroutine(ChangeImage(index, upward));
    }

    IEnumerator ChangeImage(int index, bool upward)
    {
        newImage.sprite = stageImages[index];

        // 
        if (upward)
        {
            newImage.fillOrigin = (int)Image.OriginHorizontal.Right;
        }
        // 
        else
        {
            newImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        }

        newImage.fillAmount = 0;

        float time = 0;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            newImage.fillAmount = Mathf.Clamp01(time / duration);

            yield return null;
        }

        newImage.fillAmount = 1;

        oldImage.sprite = stageImages[index];
        oldImage.enabled = true; // 

        newImage.fillAmount = 0;

        changing = null;
    }

    public void ShowFirstImage()
    {
        // 
        ChangeStage(0, false);
    }
}
