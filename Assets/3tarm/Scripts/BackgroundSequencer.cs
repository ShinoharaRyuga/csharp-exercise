using UnityEngine;

public class BackgroundSequencer : MonoBehaviour
{
    [SerializeField, Tooltip("合成処理を行うクラス")]
    private BackGroundTranslator _backGroundTransitioner = default;

    [SerializeField, Header("表示するイラストの配列")]
    Sprite[] _nextBackGroundImages = default;

    private int _currentIndex = -1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (_backGroundTransitioner is { IsLerp: false })
            {
                BackGroundMoveNext();
            }
            else
            {
                _backGroundTransitioner.Skip();
            }
        }
    }

    /// <summary>次の背景に切り替える</summary>
    public void BackGroundMoveNext()
    {
        if (_currentIndex + 1 < _nextBackGroundImages.Length)
        {
            _currentIndex++;
            _backGroundTransitioner?.Play(_nextBackGroundImages[_currentIndex]);
        }
    }
}