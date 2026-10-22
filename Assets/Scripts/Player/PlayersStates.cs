using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class PlayersStates : MonoBehaviour
{
    //Spriteを表示する
    public Image status1p;
    public Image status2p;

    public Sprite[] img;

    //プレイヤーからの取得
    public Player1Script pl1;
    public Player2Script pl2;

    //animation
    private Animator anim1;
    private Animator anim2;
    private float animDuration;

    // 前回の状態保持（状態変更時の検知用）
    private Player1Script.PlayerState lastState1P;
    private Player2Script.PlayerState lastState2P;

    private Coroutine coroutine1P;
    private Coroutine coroutine2P;

    void Start()
    {
        if(anim1 == null) anim1 = GetComponent<Animator>();
        if(anim2 == null) anim2 = anim1;

        if(pl1 !=  null)
        {
            lastState1P = pl1.state;
            Update1PVisual(pl1.state);
        }

        if(pl2 != null)
        {
            lastState2P = pl2.state;
            Update2PVisual(pl2.state);
        }

    }

    // Update is called once per frame
    void Update()
    {
        ////1pの状態（state）を取得し、imageに状態にあったイラストを表示する
        //StartCoroutine(P1ItemVisual());

        ////2pの状態（state）を取得し、imageに状態にあったイラストを表示する
        //StartCoroutine(P2ItemVisual());

        if(pl1 != null && pl1.state != lastState1P)
        {
            lastState1P = pl1.state;
            Update1PVisual(pl1.state);
        }

        if(pl2 != null && pl2.state != lastState2P)
        {
            lastState2P = pl2.state;
            Update2PVisual(pl2.state);
        }
    }

    //private IEnumerator P1ItemVisual()
    //{
    //    anim1.Play("1Pshuffling");

    //    yield return null;
    //    AnimatorStateInfo info = anim1.GetCurrentAnimatorStateInfo(0);

    //    yield return new WaitForSeconds(info.length);

    //    switch (pl1.state)
    //    {
    //        case Player1Script.PlayerState.None:
    //            status1p.sprite = img[0]; break;
    //        case Player1Script.PlayerState.SpeedUP:
    //            status1p.sprite = img[1]; break;
    //        case Player1Script.PlayerState.ScoreUP:
    //            status1p.sprite = img[2]; break;
    //        case Player1Script.PlayerState.Debuff:
    //            status1p.sprite = img[3]; break;
    //        case Player1Script.PlayerState.BigUp:
    //            status1p.sprite = img[4]; break;
    //    }

    //    anim1.Play("1PitemResult");
    //}

    //private IEnumerator P2ItemVisual()
    //{
    //    anim2.Play("2Pshuffling");

    //    yield return null;
    //    AnimatorStateInfo info = anim2.GetCurrentAnimatorStateInfo(0);

    //    yield return new WaitForSeconds(info.length);

    //    switch (pl2.state)
    //    {
    //        case Player2Script.PlayerState.None:
    //            status2p.sprite = img[0]; break;
    //        case Player2Script.PlayerState.SpeedUP:
    //            status2p.sprite = img[1]; break;
    //        case Player2Script.PlayerState.ScoreUP:
    //            status2p.sprite = img[2]; break;
    //        case Player2Script.PlayerState.Debuff:
    //            status2p.sprite = img[3]; break;
    //        case Player2Script.PlayerState.BigUp:
    //            status1p.sprite = img[4]; break;
    //    }

    //    anim2.Play("2PitemResult");
    //}

    //----------------------------------------------------------------------------

    private void Update1PVisual(Player1Script.PlayerState state)
    {
        if (coroutine1P != null) StopCoroutine(coroutine1P);

        if(state == Player1Script.PlayerState.None)
        {
            // Noneの場合はアニメーションを再生（ルーピング）
            status1p.sprite = GetSpriteByState1P(Player1Script.PlayerState.None);
            if (anim1 != null) anim1.Play("1Pshuffling", 0, 0f);
        }
        else
        {
            coroutine1P = StartCoroutine(StopP1AnimationAndShowResult(state));
        }
    }

    private IEnumerator StopP1AnimationAndShowResult(Player1Script.PlayerState state)
    {
        if (anim1 != null)
        {
            // 結果表示用のアニメーション（静止または演出）に切り替え
            anim1.Play("1PitemResult", 0, 0f);
        }

        yield return null;
        if (status1p != null )
        {
            // 画像の確定
            status1p.sprite = GetSpriteByState1P(state);
        }


    }
    //-------------------------------------------------------------------------

    private void Update2PVisual(Player2Script.PlayerState state)
    {
        if (coroutine2P != null) StopCoroutine(coroutine2P);

        if (state == Player2Script.PlayerState.None)
        {
            // Noneの場合はアニメーションを再生（ルーピング）
            status2p.sprite = GetSpriteByState2P(Player2Script.PlayerState.None);
            if (anim2 != null) anim2.Play("2Pshuffling", 1, 0f);
        }
        else
        {
            coroutine2P = StartCoroutine(StopP2AnimationAndShowResult(state));
        }
    }

    private IEnumerator StopP2AnimationAndShowResult(Player2Script.PlayerState state)
    {
        if (anim2 != null)
        {
            // 結果表示用のアニメーション（静止または演出）に切り替え
            anim2.Play("2PitemResult", 1, 0f);
        }
        
        yield return null;
        // 画像の確定
        if(status2p != null)
        {
            status2p.sprite = GetSpriteByState2P(state);
        }


    }

    private Sprite GetSpriteByState1P(Player1Script.PlayerState state)
    {
        Sprite selectedSprite = img[0];
        switch (state)
        {
            case Player1Script.PlayerState.None: selectedSprite = img[0]; break;
            case Player1Script.PlayerState.ScoreUP: selectedSprite = img[1]; break;
            case Player1Script.PlayerState.SpeedUP: selectedSprite = img[2]; break;
            case Player1Script.PlayerState.Debuff: selectedSprite = img[3]; break;
            case Player1Script.PlayerState.BigUp: selectedSprite = img[4]; break;
            default: selectedSprite = img[0]; break;
        }

        Debug.Log($"[1P State確認] State: {state} ➔ 画像名: {(selectedSprite != null ? selectedSprite.name : "null")}");
        return selectedSprite;
    }

    private Sprite GetSpriteByState2P(Player2Script.PlayerState state)
    {
        Sprite selectedSprite = img[0];

        switch (state)
        {
            case Player2Script.PlayerState.None: selectedSprite = img[0]; break;
            case Player2Script.PlayerState.ScoreUP: selectedSprite = img[1]; break;
            case Player2Script.PlayerState.SpeedUP: selectedSprite = img[2]; break;
            case Player2Script.PlayerState.Debuff: selectedSprite = img[3]; break;
            case Player2Script.PlayerState.BigUp: selectedSprite = img[4]; break;
            default: selectedSprite = img[0]; break;
        }

        Debug.Log($"[2P State確認] State: {state} ➔ 画像名: {(selectedSprite != null ? selectedSprite.name : "null")}");
        return selectedSprite;
    }
}
