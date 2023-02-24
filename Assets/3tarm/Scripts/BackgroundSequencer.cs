using UnityEngine;

public class BackgroundSequencer : MonoBehaviour
{
    [SerializeField, Tooltip("�����������s���N���X")]
    private BackGroundTranslator _backGroundTransitioner = default;

    [SerializeField, Header("�\������C���X�g�̔z��")]
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

    /// <summary>���̔w�i�ɐ؂�ւ���</summary>
    public void BackGroundMoveNext()
    {
        if (_currentIndex + 1 < _nextBackGroundImages.Length)
        {
            _currentIndex++;
            _backGroundTransitioner?.Play(_nextBackGroundImages[_currentIndex]);
        }
    }
}