using UnityEngine;
using UnityEngine.UI;

/// <summary>背景合成を行うクラス</summary>
public class BackGroundTranslator : MonoBehaviour
{
    /// <summary>フェードを行う為のcolor <see cref="_currentfrom"/> </summary>
    readonly Color CURRENT_BACKGROUND_COLOR = new Color(0, 0, 0, 0);
    /// <summary>フェードを行う為のcolor <see cref="_nextfrom"/> </summary>
    readonly Color NEXT_BACKGROUND_COLOR = new Color(1, 1, 1, 1);

    [SerializeField, Tooltip("表示中の背景")]
    Image _currentImage = default;

    [SerializeField, Tooltip("フェード用のイメージ")]
    Image _fadeImage = default;

    [SerializeField, Tooltip("遷移時間")]
    float _duration = 1f;

    /// <summary>経過時間 </summary>
    float _elapsed = 0;
    /// <summary>Lerp処理を行っている </summary>
    bool _isLerp = false;
    /// <summary>現在表示している背景の色 </summary>
    Color _currentfrom;
    /// <summary>次に表示する背景の色</summary>
    Color _nextfrom;

    public bool IsLerp { get => _isLerp; }

    void Update()
    {
        if (_fadeImage.sprite == null) { return; }

        _elapsed += Time.deltaTime;

        if (_elapsed < _duration)
        {
            _currentImage.color = Color.Lerp(_currentfrom, CURRENT_BACKGROUND_COLOR, _elapsed / _duration);
            _fadeImage.color = Color.Lerp(_nextfrom, NEXT_BACKGROUND_COLOR, _elapsed / _duration);
        }
        else if (_isLerp)
        {
            ResetProcess();
        }
    }

    /// <summary>再度処理を行えるようにする </summary>
    void ResetProcess()
    {
        _currentImage.sprite = _fadeImage.sprite;
        _fadeImage.sprite = null;                       //処理を走らせないようにする為
        _fadeImage.color = CURRENT_BACKGROUND_COLOR;    //透明にする
        _currentImage.color = NEXT_BACKGROUND_COLOR;
        _isLerp = false;
    }

    /// <summary>背景合成処理を開始する</summary>
    /// <param name="backGround"></param>
    public void Play(Sprite backGround)
    {
        if (_fadeImage is null || _currentImage is null) { return; }

        _fadeImage.sprite = backGround;
        _currentfrom = _currentImage.color;
        _nextfrom = _fadeImage.color;
        _elapsed = 0;
        _isLerp = true;
    }

    public void Skip()
    {
        ResetProcess();
    }
}
