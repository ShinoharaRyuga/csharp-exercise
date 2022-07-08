using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームマネジャー 
/// プレイヤーの入力を受け取る
/// 盤面を生成する
/// </summary>
public class MineSweeper : MonoBehaviour
{
    [SerializeField, Tooltip("縦の長さ")] int _rows = 10;
    [SerializeField, Tooltip("横の長さ")] int _columns = 10;
    [SerializeField, Tooltip("地雷の数")] int _mineCount = 10;
    [SerializeField, Tooltip("地雷のが見えるかどうか")] bool _isView = false;
    [SerializeField] GridLayoutGroup _gridLayoutGroup = null;
    [SerializeField, Tooltip("セルのプレハブ")] Cell _cellPrefab = null;
    [SerializeField, Tooltip("経過時間を表示するテキスト")] Text _timeText = null;
    int _currentFlagCount = 0;
    float _gameTime = 0;
    bool _isGame = true;
    bool _isTimerStart = false;
    bool _isfirst = true;
    Cell[,] _cells;

    private void Start()
    {
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = _columns;
        _cells = new Cell[_rows, _columns];

        //盤面を生成する
        for (var r = 0; r < _rows; r++)
        {
            for (var c = 0; c < _columns; c++)
            {
                var cell = Instantiate(_cellPrefab, _gridLayoutGroup.transform);
                cell.SetView(_isView);
                cell.Row = r;
                cell.Column = c;
                _cells[r, c] = cell;
            }
        }

        SetMine();
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
                var cell = hit.collider.gameObject.GetComponent<Cell>();
     
                if (_isfirst)
                {
                    _isTimerStart = true;
                    _isfirst = false;
                    if (cell.CellState == CellState.Mine)
                    {
                        ResetMinePos();
                        SetMine(cell);
                    }
                }

                AroundCell(cell);
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

            if (_isTimerStart)  //時間計測を開始する
            {
                _gameTime += Time.deltaTime;
                _timeText.text = _gameTime.ToString();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))    //地雷の位置を見えるようにする
        {
            foreach (var cell in _cells)
            {
                cell.SetView(!_isView);
            }

            _isView = !_isView;
        }

        if (Input.GetKeyDown(KeyCode.C))    //リトライ
        {
            ResetMinePos();
            SetMine();
        }
    }

    /// <summary>ゲームクリアしているから調べる </summary>
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

    /// <summary>時間計測を止める </summary>
    private void GameClear()
    {
        _isTimerStart = false;
        _isGame = false;
        Debug.Log("クリア");
    }

    /// <summary>地雷の引いたらゲームを終了させる </summary>
    /// <param name="gameover">true=開けたセルが地雷である false=地雷ではない</param>
    private void GameOver(bool gameover)
    {
        if (gameover)
        {
            Debug.Log("失敗");
            _isGame = false;
        }
    }

    /// <summary>地雷をランダムにセットする </summary>
    private void SetMine()
    {
        var count = Mathf.Min(_mineCount, _cells.Length);

        for (var i = 0; i < count;)
        {
            var r = UnityEngine.Random.Range(0, _rows);
            var c = UnityEngine.Random.Range(0, _columns);
            var cell = _cells[r, c];

            if (cell.CellState != CellState.Mine)　//選択されたセルの状態が空であれば地雷にする
            {
                i++;
                cell.CellState = CellState.Mine;
                SetMineCount(r, c);
            }
        }
    }

    /// <summary>プレイヤーが一手目に地雷を引いたら地雷を再セットする</summary>
    private void SetMine(Cell firstCell)
    {
        for (var i = 0; i < _mineCount; i++)
        {
            var r = UnityEngine.Random.Range(0, _rows);
            var c = UnityEngine.Random.Range(0, _columns);
            var cell = _cells[r, c];

            if (cell.CellState == CellState.None && firstCell != cell)
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

    /// <summary>セルの状態を空の状態にする</summary>
    private void ResetMinePos()
    {
        foreach (var cell in _cells)
        {
            cell.ResetState();
        }
    }
    
    /// <summary>地雷の数を数える </summary>
    /// <param name="r"></param>
    /// <param name="c"></param>
    private void SetMineCount(int r, int c)
    {
        if (0 <= r - 1 && 0 <= c - 1)   //左上
        {
            CheckMine(r - 1, c - 1);
        }

        if (0 <= r - 1) //上
        {
            CheckMine(r - 1, c);
        }

        if (0 <= r - 1 && c + 1 < _columns) //右上
        {
            CheckMine(r - 1, c + 1);
        }

        if (0 <= c - 1) //左
        {
            CheckMine(r, c - 1);
        }

        if (c + 1 < _columns)   //右
        {
            CheckMine(r, c + 1);
        }


        if (r + 1 < _rows && 0 <= c - 1) //左下
        {
            CheckMine(r + 1, c - 1);
        }

        if (r + 1 < _rows) //下
        {
            CheckMine(r + 1, c);
        }

        if (r + 1 < _rows && c + 1 < _columns) //右下
        {
            CheckMine(r + 1, c + 1);
        }
    }

    /// <summary>引数を使いセルを取得しのそのセル状態が地雷ではなければ周囲の地雷の数を表示するにする</summary>
    /// <param name="r"></param>
    /// <param name="c"></param>
    private void CheckMine(int r, int c)
    {
        var cell = _cells[r, c];
        if (cell.CellState != CellState.Mine)
        {
            cell.MineCount++;
            cell.CellState = CellState.Count;
        }
    }

        /// <summary>開いたセルの周囲八マスを開ける </summary>
        /// <param name="target"></param>
    private void AroundCell(Cell target)
    {
        int r = target.Row;
        int c = target.Column;

        if (0 <= r - 1 && 0 <= c - 1)   //左上
        {
            var cell = _cells[r - 1, c - 1];
            cell.OpenCell();
            GameOver(cell.OpenCell());
        }

        if (0 <= r - 1) //上
        {
            var cell = _cells[r - 1, c];
            GameOver(cell.OpenCell());
        }

        if (0 <= r - 1 && c + 1 < _columns) //右上
        {
            var cell = _cells[r - 1, c + 1];
            GameOver(cell.OpenCell());
        }

        if (0 <= c - 1) //左
        {
            var cell = _cells[r, c - 1];
            GameOver(cell.OpenCell());
        }

        if (c + 1 < _columns)   //右
        {
            var cell = _cells[r, c + 1];
            GameOver(cell.OpenCell());
        }


        if (r + 1 < _rows && 0 <= c - 1) //左下
        {
            var cell = _cells[r + 1, c - 1];
            GameOver(cell.OpenCell());
        }

        if (r + 1 < _rows) //下
        {
            var cell = _cells[r + 1, c];
            GameOver(cell.OpenCell());
        }

        if (r + 1 < _rows && c + 1 < _columns) //右下
        {
            var cell = _cells[r + 1, c + 1];
            GameOver(cell.OpenCell());
        }
    }
}
