using UnityEngine;
using UnityEngine.UI;

public class ColorTranslator : MonoBehaviour
{
    [SerializeField]
    Image _image = default;
    [SerializeField, Tooltip("目標カラー")]
    Color _to = default;
    [SerializeField, Tooltip("遷移時間")]
    float _duration = 1f;

    Color _from;
    /// <summary>経過時間 </summary>
    private float _elapsed = 0;

    /// <summary>背景色の遷移が完了しているかどうか</summary>
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

    /// <summary> 背景色の遷移処理を開始する</summary>
    /// <param name="color">次の色</param>
    public void Play(Color color)
    {
        if (_image is null) { return; }

        _from = _image.color;
        _to = color;
        _elapsed = 0;
    }

    /// <summary> 現在の背景色遷移処理をスキップする</summary>
    public void Skip()
    {
        _elapsed = _duration;
    }
}
