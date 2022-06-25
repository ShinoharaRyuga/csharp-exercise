using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MineSweeper : MonoBehaviour
{
    [SerializeField] int _rows = 10;
    [SerializeField] int _columns = 10;
    [SerializeField] int _mineCount = 10;
    [SerializeField] bool _isView = false;
    [SerializeField] GridLayoutGroup _gridLayoutGroup = null;
    [SerializeField] Cell _cellPrefab = null;
    [SerializeField] Text _timeText = null;

    int _currentFlagCount = 0;
    float _gameTime = 0;
    bool _isGame = true;
    bool _isTimerStart = false;
    Cell[,] _cells;
    private void Start()
    {
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = _columns;
        _cells = new Cell[_rows, _columns];

        for (var r = 0; r < _rows; r++)
        {
            for (var c = 0; c < _columns; c++)
            {
                var cell = Instantiate(_cellPrefab, _gridLayoutGroup.transform);
                cell.SetView(_isView);
                _cells[r, c] = cell;
            }
        }

        for (var i = 0; i < _mineCount; i++)
        {
            var r = Random.Range(0, _rows);
            var c = Random.Range(0, _columns);
            var cell = _cells[r, c];

            if (cell.CellState == CellState.None)
            {
                cell.CellState = CellState.Mine;
                SetMineCount(r, c);
            }
            else
            {
                i--;
            }
        }
    }

    private void Update()
    {
        var screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

        if (_isGame)
        {
            if (Input.GetButtonDown("Fire1") && hit)   //セルを開ける
            {
                if (!_isTimerStart) _isTimerStart = true;
                var cell = hit.collider.gameObject.GetComponent<Cell>();
                GameOver(cell.OpenCell());
                GameClearCheck();
            }
            else if (Input.GetButtonDown("Fire2") && hit)  //旗を立てる
            {
                if (!_isTimerStart) _isTimerStart = true;
                var cell = hit.collider.gameObject.GetComponent<Cell>();
                var Image = hit.collider.gameObject.GetComponent<Image>();
                
                if (cell.IsFlag)
                {
                    cell.IsFlag = false;
                    Image.color = Color.white;
                }
                else
                {
                    cell.IsFlag = true;
                    Image.color = Color.red;
                }
               
                GameClearCheck();
            }

            if (_isTimerStart)
            {
                _gameTime += Time.deltaTime;
                _timeText.text = _gameTime.ToString();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var cell in _cells)
            {
                cell.SetView(!_isView);
            }

            _isView = !_isView;
        }
    }

    private void GameClearCheck()
    {
        foreach (var cell in _cells)
        {
            if (cell.CellState == CellState.Mine && cell.IsFlag)
            {
                _currentFlagCount++;
                continue;
            }

            if (cell.IsOpen == false)
            {
                _currentFlagCount = 0;
                return;
            }
        }

        if (_currentFlagCount == _mineCount)
        {
            GameClear();
        }
    }

    private void GameClear()
    {
        _isTimerStart = false;
        _isGame = false;
        Debug.Log("クリア");
    }

    private void GameOver(bool gameover)
    {
        if (gameover)
        {
            Debug.Log("失敗");
            _isGame = false;
        }
    }
    

    private void SetMineCount(int r, int c)
    {
        if (0 <= r - 1 && 0 <= c - 1)   //左上
        {
            var cell = _cells[r - 1, c - 1];
            cell.MineCount++;
            cell.CellState = CellState.Count;
        }

        if (0 <= r - 1) //上
        {
            var cell = _cells[r - 1, c];
            cell.MineCount++;
            cell.CellState = CellState.Count;
        }

        if (0 <= r - 1 && c + 1 < _columns) //右上
        {
            var cell = _cells[r - 1, c + 1];
            cell.MineCount++;
            cell.CellState = CellState.Count;
        }

        if (0 <= c - 1) //左
        {
            var cell = _cells[r, c - 1];
            cell.MineCount++;
            cell.CellState = CellState.Count;
        }

        if (c + 1 < _columns)   //右
        {
            var cell = _cells[r, c + 1];
            cell.MineCount++;
            cell.CellState = CellState.Count;
        }


        if (r + 1 < _rows && 0 <= c - 1) //左下
        {
            var cell = _cells[r + 1, c - 1];
            cell.MineCount++;
            cell.CellState = CellState.Count;
        }

        if (r + 1 < _rows) //下
        {
            var cell = _cells[r + 1, c];
            cell.MineCount++;
            cell.CellState = CellState.Count;
        }

        if (r + 1 < _rows && c + 1 < _columns) //右下
        {
            var cell = _cells[r + 1, c + 1];
            cell.MineCount++;
            cell.CellState = CellState.Count;
        }
    }
}
