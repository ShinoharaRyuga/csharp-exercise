using UnityEngine;
using UnityEngine.UI;

public class BackGroundTranslator : MonoBehaviour
{
    [SerializeField, Tooltip("表示中の背景")]
    Image _currentImage = default;

    [SerializeField, Tooltip("フェード用のイメージ")]
    Image _fadeImage = default;

    [SerializeField, Tooltip("遷移時間")]
    float _duration = 1f;

    /// <summary>経過時間 </summary>
    private float _elapsed = 0;

    Color _currentfrom;
    Color _nextfrom;

    void Update()
    {
        if(_fadeImage.sprite == null) { return; }

        _elapsed += Time.deltaTime;
       
        if (_elapsed < _duration)
        {
            _currentImage.color = Color.Lerp(_currentfrom, new Color(0, 0, 0, 0), _elapsed / _duration);
            _fadeImage.color = Color.Lerp(_nextfrom, new Color(1, 1, 1, 1), _elapsed / _duration);
        }
        else
        {
            _currentImage.color = _fadeImage.color;
        }
    }

    public void Play(Sprite backGround)
    {
        if (_fadeImage is null || _currentImage is null) { return; }

        _fadeImage.sprite = backGround;
        _currentfrom = _currentImage.color;
        _nextfrom = _fadeImage.color;
    }
}
