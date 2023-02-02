using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>�L�����N�^�[�̉摜�𑀍삷��N���X </summary>
public class Actor : MonoBehaviour
{
    [SerializeField]
    private Image _image = default;

    public IEnumerator FadeIn(float time)
    {
        var color = _image.color;

        // color �̃A���t�@�l�����X�� 1 �ɋ߂Â��鏈��
        var elapsed = 0F;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            color.a = elapsed / time;
            _image.color = color;
            yield return null;
        }

        color.a = 1;
        _image.color = color;
        yield return null;
    }

    public IEnumerator FadeOut(float time)
    {
        var color = _image.color;

        // color �̃A���t�@�l�����X�� 0 �ɋ߂Â��鏈��
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

    /// <summary>�\���������C���X�g��ǂݍ��݁A�C���X�g��ύX���� </summary>
    /// <param name="character">�ǂ̃L�����N�^�[�̃C���X�g��</param>
    /// <param name="face">�\�������������̓Y����</param>
    public void ChangeCharacterImage(Characters character, FaceSprites face)
    {
        var diffSprite = Resources.Load<Sprite>($"Sprites/{character}/{face}");
        _image.sprite = diffSprite;
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
