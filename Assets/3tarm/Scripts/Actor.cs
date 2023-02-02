using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>キャラクターの画像を操作するクラス </summary>
public class Actor : MonoBehaviour
{
    [SerializeField]
    private Image _image = default;

    /// <summary>キャラクターを登場させる</summary>
    /// <param name="time">登場にかかる時間</param>
    /// <param name="endAction">終了処理</param>
    public IEnumerator FadeIn(float time, Action endAction = null)
    {
        var color = _image.color;
        if (!_image.enabled) { _image.enabled = true; }

        var elapsed = 0F;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            color.a = elapsed / time;
            _image.color = color;
            yield return null;
        }

        endAction?.Invoke();
        color.a = 1;
        _image.color = color;
        yield return null;
    }


    /// <summary>キャラクターを退場させる</summary>
    /// <param name="time">退場にかかる時間</param>
    public IEnumerator FadeOut(float time)
    {
        var color = _image.color;

        // color のアルファ値を徐々に 0 に近づける処理
        var elapsed = 0F;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            color.a = 1 - elapsed / time;
            _image.color = color;
            yield return null;
        }

        color.a = 0;
        _image.color = color;
        yield return null;
    }

    /// <summary>表示したいイラストを読み込み、イラストを変更する </summary>
    /// <param name="character">どのキャラクターのイラストか</param>
    /// <param name="face">表示したい差分の添え字</param>
    public void ChangeCharacterImage(Characters character, FaceSprites face)
    {
        var diffSprite = Resources.Load<Sprite>($"Sprites/{character}/{face}");
        _image.sprite = diffSprite;
    }
}

/// <summary>表情差分</summary>
public enum FaceSprites 
{
    /// <summary>通常 </summary>
    Normal = 0,
    /// <summary>自慢げ </summary>
    Boast = 1,
    /// <summary>怒り </summary>
    Anger = 2,
    /// <summary>驚き </summary>
    Surprise = 3,
    /// <summary>喜び </summary>
    Joy = 4,
    /// <summary>照れる </summary>
    Shy = 5,
}

/// <summary>登場人物</summary>
public enum Characters
{
    UnityChan = 0,
    Misaki = 1,
    Yuko = 2,
}

/// <summary>キャラクター登場・退場 </summary>
public enum Fade
{
    /// <summary>登場 </summary>
    FadeIn,
    /// <summary>退場 </summary>
    FadeOut,
}
