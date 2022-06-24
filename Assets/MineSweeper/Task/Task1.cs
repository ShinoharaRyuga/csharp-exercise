using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Task1 : MonoBehaviour
{
    [SerializeField, Tooltip("セルの数")] int _count = 0;
    /// <summary>選択中のセルの要素数 </summary>
    int _currentIndex = 0;
    /// <summary>セルの配列 </summary>
    GameObject[] _sells = null;
    private void Start()
    {
        _sells = new GameObject[_count];

        for (var i = 0; i < _count; i++)
        {
            var obj = new GameObject($"Cell{i}");
            obj.transform.parent = transform;
            _sells[i] = obj;
            var image = obj.AddComponent<Image>();
            if (i == 0) { image.color = Color.red; }
            else { image.color = Color.white; }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && _currentIndex > 0) // 左キーを押した
        {
            for (var i = _currentIndex - 1; i >= 0; i--)
            {
                if (_sells[i] != null)
                {
                    _sells[_currentIndex].GetComponent<Image>().color = Color.white;
                    _sells[i].GetComponent<Image>().color = Color.red;
                    _currentIndex = i;
                    break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && _currentIndex < _count - 1) // 右キーを押した
        {
            for (var i = _currentIndex + 1; i <= _count - 1; i++)
            {
                if (_sells[i] != null)
                {
                    _sells[_currentIndex].GetComponent<Image>().color = Color.white;
                    _sells[i].GetComponent<Image>().color = Color.red;
                    _currentIndex = i;
                    break;
                }
            }
        }

        if (Input.GetButtonDown("Jump") && _sells[_currentIndex])
        {
            Destroy(_sells[_currentIndex]);
            StartCoroutine(SetSell());
           
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(_currentIndex);

            foreach (var sell in _sells)
            {
                Debug.Log(sell);
            }
        }
    }

    IEnumerator SetSell()
    {
        yield return null;

        for (var i = 0; i < _count; i++)
        {
            if (_sells[i])
            {
                _sells[i].GetComponent<Image>().color = Color.red;
                _currentIndex = i;
                break;
            }
        }
    }
}
