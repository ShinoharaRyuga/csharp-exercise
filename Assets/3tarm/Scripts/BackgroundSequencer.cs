using UnityEngine;

public class BackgroundSequencer : MonoBehaviour
{
    [SerializeField]
    private BackGroundTranslator _backGroundTransitioner = default;

    [SerializeField,]
    Sprite[] _nextBackGroundImage = default;

    private int _currentIndex = -1;

    void Start()
    {
     //   MoveNext();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //if (_backGroundTransitioner is { IsCompleted: false })
            //{
            //    _backGroundTransitioner.Skip();
            //}
            //else { MoveNext(); }

            _backGroundTransitioner?.Play(_nextBackGroundImage[0]);
        }
    }

    private void MoveNext()
    {
        //if (_colors is null or { Length: 0 }) { return; }

        //if (_currentIndex + 1 < _colors.Length)
        //{
        //    _currentIndex++;
        _backGroundTransitioner?.Play(_nextBackGroundImage[0]);
        //}
    }
}