using UnityEngine;
using UnityEngine.UI;

public class ColorTranslator : MonoBehaviour
{
    [SerializeField]
    Image _image = default;
    [SerializeField, Tooltip("�ڕW�J���[")]
    Color _to = default;
    [SerializeField, Tooltip("�J�ڎ���")]
    float _duration = 1f;

    Color _from;
    /// <summary>�o�ߎ��� </summary>
    private float _elapsed = 0;

    /// <summary>�w�i�F�̑J�ڂ��������Ă��邩�ǂ���</summary>
    public bool IsCompleted => _image is null ? false : _image.color == _to;

    void Start()
    {
        if (_image is null)
        {
            return;
        }
        else
        {
            _from = _image.color;
        }
    }

    void Update()
    {
        _elapsed += Time.deltaTime;

        if (_elapsed < _duration)
        {
            _image.color = Color.Lerp(_from, _to, _elapsed / _duration);
        }
        else
        {
            _image.color = _to;
        }
    }

    /// <summary> �w�i�F�̑J�ڏ������J�n����</summary>
    /// <param name="color">���̐F</param>
    public void Play(Color color)
    {
        if (_image is null) { return; }

        _from = _image.color;
        _to = color;
        _elapsed = 0;
    }

    /// <summary> ���݂̔w�i�F�J�ڏ������X�L�b�v����</summary>
    public void Skip()
    {
        _elapsed = _duration;
    }
}
