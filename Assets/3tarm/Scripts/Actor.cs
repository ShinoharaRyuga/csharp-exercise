using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>キャラクターの画像を操作するクラス </summary>
public class Actor : MonoBehaviour
{
    [SerializeField, Tooltip("イラストを表示させる画像")]
    Image _image = default;

    /// <summary>表示しているキャラクター</summary>
    Characters _currentCharacter = default;
    /// <summary>現在の表情</summary>
    FaceSprites _currentFace = default;
    /// <summary>フェードを行っているかどうか 行っていれば,どのフェードを行っているかを保持している</summary>
    FadeMode _fadeMode = FadeMode.None;

    /// <summary>キャラクターを登場させる</summary>
    /// <param name="time">登場にかかる時間</param>
    /// <param name="endAction">終了処理</param>
    public IEnumerator FadeIn(float time, Action endAction = null)
    {
        var color = _image.color;
        _fadeMode = FadeMode.FadeIn;

        if (!_image.enabled) { _image.enabled = true; }     //imageが表示されていなければ表示する

        var elapsed = 0f;
        while (elapsed < time && _fadeMode is FadeMode.FadeIn)
        {
            elapsed += Time.deltaTime;
            color.a = elapsed / time;
            _image.color = color;
            yield return null;
        }

        endAction?.Invoke();
        color.a = 1;
        _image.color = color;
        _fadeMode = FadeMode.None;
        yield return null;
    }


    /// <summary>キャラクターを退場させる</summary>
    /// <param name="time">退場にかかる時間</param>
    public IEnumerator FadeOut(float time)
    {
        var color = _image.color;
        _fadeMode = FadeMode.FadeOut;

        var elapsed = 0f;
        while (elapsed < time && _fadeMode is FadeMode.FadeOut)
        {
            elapsed += Time.deltaTime;
            color.a = 1 - elapsed / time;
            _image.color = color;
            yield return null;
        }

        color.a = 0;
        _image.color = color;
        _fadeMode = FadeMode.None;
        yield return null;
    }

    /// <summary>キャラクターを登場させる時に呼び出す </summary>
    /// <param name="character">どのキャラクターのイラストを表示するか</param>
    /// <param name="face">表示したい差分の添え字</param>
    public void SetFirstCharacterSprite(Characters character, FaceSprites face)
    {
        var diffSprite = Resources.Load<Sprite>($"Sprites/{character}/{face}");
        _image.sprite = diffSprite;
        _currentCharacter = character;
        _currentFace = face;
    }

    /// <summary>表情を変更する </summary>
    /// <param name="nextFace">次の表情</param>
    public void UpdateFaceSprite(FaceSprites nextFace)
    {
        var diffSprite = Resources.Load<Sprite>($"Sprites/{_currentCharacter}/{nextFace}");
        _image.sprite = diffSprite;
    }

    public void Skip()
    {
        if (_fadeMode is FadeMode.FadeIn)
        {
            var color = _image.color;
            color.a = 1;
            _image.color = color; 
        }
        else if (_fadeMode is FadeMode.FadeOut)
        {
            var color = _image.color;
            color.a = 0;
            _image.color = color;
        }

        _fadeMode = FadeMode.None;
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
public enum FadeMode
{
    /// <summary>フェードを行っていない</summary>
    None,
    /// <summary>登場 </summary>
    FadeIn,
    /// <summary>退場 </summary>
    FadeOut,
}
