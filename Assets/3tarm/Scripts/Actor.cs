using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Actor : MonoBehaviour
{
    [SerializeField]
    private Image _image = default;

    public IEnumerator FadeIn(float time)
    {
        Debug.Log($"Actor FadeIn: time={time}", this);
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
        Debug.Log($"Actor FadeOut: time={time}", this);
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
}