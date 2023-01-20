using UnityEngine;
using UnityEngine.UI;

public class ColorTranslator : MonoBehaviour
{
    [SerializeField]
    Image _image = default;
    [SerializeField, Tooltip("–Ú•WƒJƒ‰[")]
    Color _to = default;
    [SerializeField, Tooltip("‘JˆÚŠÔ")]
    float _duration = 1f;

    Color _from;
    /// <summary>Œo‰ßŠÔ </summary>
    private float _elapsed = 0;

    /// <summary>”wŒiF‚Ì‘JˆÚ‚ªŠ®—¹‚µ‚Ä‚¢‚é‚©‚Ç‚¤‚©</summary>
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

    /// <summary> ”wŒiF‚Ì‘JˆÚˆ—‚ğŠJn‚·‚é</summary>
    /// <param name="color">Ÿ‚ÌF</param>
    public void Play(Color color)
    {
        if (_image is null) { return; }

        _from = _image.color;
        _to = color;
        _elapsed = 0;
    }

    /// <summary> Œ»İ‚Ì”wŒiF‘JˆÚˆ—‚ğƒXƒLƒbƒv‚·‚é</summary>
    public void Skip()
    {
        _elapsed = _duration;
    }
}
