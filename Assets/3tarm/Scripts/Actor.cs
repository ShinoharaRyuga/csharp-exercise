using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


interface IResult<T>
{
    /// <summary> �������I���������ǂ���</summary>
    bool IsCompleted { get; }

    /// <summary>����</summary>
    T Result { get; }
}

/// <summary>���ʂ�ݒ肷��N���X</summary>
public class ResultClass<T> : IResult<T>
{
    public bool IsCompleted { get; private set; }

    public T Result { get; private set; }

    public void SetResult(T result)
    {
        Result = result;
        IsCompleted = true;
    }
}

/// <summary>�L�����N�^�[�̉摜�𑀍삷��N���X </summary>
public class Actor : MonoBehaviour
{
    [SerializeField, Tooltip("�C���X�g��\��������摜")]
    Image _image = default;

    /// <summary>�t�F�[�h�����̌��� </summary>
    ResultClass<bool> _fadeResult;

    /// <summary>�\�����Ă���L�����N�^�[</summary>
    Characters _currentCharacter = default;
    /// <summary>���݂̕\��</summary>
    FaceSprites _currentFace = default;
    /// <summary>�t�F�[�h���s���Ă��邩�ǂ��� �s���Ă����,�ǂ̃t�F�[�h���s���Ă��邩��ێ����Ă���</summary>
    FadeMode _fadeMode = FadeMode.None;

    /// <summary>�t�F�[�h�����̌��� </summary>
    public bool FadeResult { get => _fadeResult.Result; }

    public IEnumerator FadeRun(FadeMode mode, float time)
    {
        if (mode is FadeMode.FadeIn)
        {
            return FadeIn(time, out var _);
        }
        else 
        {
           return FadeOut(time);
        }
    }

    IEnumerator FadeIn(float time, out IResult<bool> result)
    {
        var impl = new ResultClass<bool>();
        var e = FadeIn(time, impl);
        result = impl;
        return e;
    }

    /// <summary>�L�����N�^�[��o�ꂳ����</summary>
    /// <param name="time">�o��ɂ����鎞��</param>
    /// <param name="endAction">�I������</param>
    public IEnumerator FadeIn(float time, ResultClass<bool> result)
    {
        var color = _image.color;
        _fadeMode = FadeMode.FadeIn;

        if (!_image.enabled) { _image.enabled = true; }     //image���\������Ă��Ȃ���Ε\������

        var elapsed = 0f;
        while (elapsed < time && _fadeMode is FadeMode.FadeIn)
        {
            elapsed += Time.deltaTime;
            color.a = elapsed / time;
            _image.color = color;
            yield return null;
        }

        result.SetResult(true);
        color.a = 1;
        _image.color = color;
        _fadeMode = FadeMode.None;
        yield return null;
    }


    /// <summary>�L�����N�^�[��ޏꂳ����</summary>
    /// <param name="time">�ޏ�ɂ����鎞��</param>
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

    /// <summary>�L�����N�^�[��o�ꂳ���鎞�ɌĂяo�� </summary>
    /// <param name="character">�ǂ̃L�����N�^�[�̃C���X�g��\�����邩</param>
    /// <param name="face">�\�������������̓Y����</param>
    public void SetFirstCharacterSprite(Characters character, FaceSprites face)
    {
        var diffSprite = Resources.Load<Sprite>($"Sprites/{character}/{face}");
        _image.sprite = diffSprite;
        _currentCharacter = character;
        _currentFace = face;
    }

    /// <summary>�\���ύX���� </summary>
    /// <param name="nextFace">���̕\��</param>
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

/// <summary>�\���</summary>
public enum FaceSprites
{
    /// <summary>�ʏ� </summary>
    Normal = 0,
    /// <summary>������ </summary>
    Boast = 1,
    /// <summary>�{�� </summary>
    Anger = 2,
    /// <summary>���� </summary>
    Surprise = 3,
    /// <summary>��� </summary>
    Joy = 4,
    /// <summary>�Ƃ�� </summary>
    Shy = 5,
}

/// <summary>�o��l��</summary>
public enum Characters
{
    UnityChan = 0,
    Misaki = 1,
    Yuko = 2,
}

/// <summary>�L�����N�^�[�o��E�ޏ� </summary>
public enum FadeMode
{
    /// <summary>�g�p�֎~ (�t�F�[�h���s���Ă��Ȃ�)</summary>
    None,
    /// <summary>�o�� </summary>
    FadeIn,
    /// <summary>�ޏ� </summary>
    FadeOut,
}
