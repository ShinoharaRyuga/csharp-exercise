using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Task2 : MonoBehaviour
{
    [SerializeField, Tooltip("縦列のセル数")] int _row = 3;
    [SerializeField, Tooltip("横列のセル数")] int _column = 4;
    /// <summary>縦、横の順番で初期化する </summary>
    GameObject[,] _sells = default;

    int _currentRowIndex = 0;
    int _currentColumnIndex = 0;
    private void Start()
    {
        _sells = new GameObject[_row, _column];
        var layout = GetComponent<GridLayoutGroup>();
        layout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        layout.constraintCount = _column;

        for (var r = 0; r < _row; r++)
        {
            for (var c = 0; c < _column; c++)
            {
                var obj = new GameObject($"Cell({r}, {c})");
                obj.transform.parent = transform;
                var image = obj.AddComponent<Image>();
                _sells[r, c] = obj;
                if (r == 0 && c == 0) { image.color = Color.red; }
                else { image.color = Color.white; }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) // 左キーを押した
        {
            for (var i = _currentColumnIndex - 1; i >= 0; i--)
            {
                if (_sells[_currentRowIndex, i].GetComponent<Image>().enabled == true)
                {
                    _sells[_currentRowIndex, _currentColumnIndex].GetComponent<Image>().color = Color.white;
                    _sells[_currentRowIndex, i].GetComponent<Image>().color = Color.red;
                    _currentColumnIndex = i;
                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) // 右キーを押した
        {
            for (var i = _currentColumnIndex + 1; i < _sells.GetLength(1); i++)
            {
                if (_sells[_currentRowIndex, i].GetComponent<Image>().enabled == true)
                {
                    _sells[_currentRowIndex, _currentColumnIndex].GetComponent<Image>().color = Color.white;
                    _sells[_currentRowIndex, i].GetComponent<Image>().color = Color.red;
                    _currentColumnIndex = i;
                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) // 上キーを押した
        {
            for (var i = _currentRowIndex - 1; i >= 0; i--)
            {
                if (_sells[i, _currentColumnIndex].GetComponent<Image>().enabled == true)
                {
                    _sells[_currentRowIndex, _currentColumnIndex].GetComponent<Image>().color = Color.white;
                    _sells[i, _currentColumnIndex].GetComponent<Image>().color = Color.red;
                    _currentRowIndex = i;
                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) // 下キーを押した
        {
            for (var i = _currentRowIndex + 1; i < _sells.GetLength(0); i++)
            {
                if (_sells[i, _currentColumnIndex].GetComponent<Image>().enabled == true)
                {
                    _sells[_currentRowIndex, _currentColumnIndex].GetComponent<Image>().color = Color.white;
                    _sells[i, _currentColumnIndex].GetComponent<Image>().color = Color.red;
                    _currentRowIndex = i;
                    break;
                }
            }
        }

        if (Input.GetButtonDown("Jump")) //削除
        {
            _sells[_currentRowIndex, _currentColumnIndex].GetComponent<Image>().enabled = false;
            // Destroy(_sells[_currentRowIndex, _currentColumnIndex]);
            // _sells[_currentRowIndex, _currentColumnIndex].SetActive(false);
            StartCoroutine(SetSell());
        }
    }

    /// <summary>セルを消した時左上からセル調べてそのセルが選択可能だったら選択する </summary>
    IEnumerator SetSell()
    {
        yield return null;
        bool endFlag = false;

        for (var r = 0; r < _row; r++)
        {
            for (var c = 0; c < _column; c++)
            {
                if (_sells[r, c].GetComponent<Image>().enabled == true)
                {
                    _sells[r, c].GetComponent<Image>().color = Color.red;
                    _currentRowIndex = r;
                    _currentColumnIndex = c;
                    endFlag = true;
                    break;
                }
            }

            if (endFlag)
            {
                break;
            }
        }
    }
}