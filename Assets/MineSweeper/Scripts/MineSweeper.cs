using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームマネジャー 
/// プレイヤーの入力を受け取る
/// 盤面を生成する
/// </summary>
public class MineSweeper : MonoBehaviour
{
    [LevelDataArray(new string[] { "横の長さ", "縦の長さ", "地雷数" })]
    [SerializeField] int[] _easyData = new int[3];
    [LevelDataArray(new string[] { "横の長さ", "縦の長さ", "地雷数" })]
    [SerializeField] int[] _nomalData = new int[3];
    [LevelDataArray(new string[] { "横の長さ", "縦の長さ", "地雷数" })]
    [SerializeField] int[] _hardData = new int[3];
    [SerializeField, Header("難易度")] Level _currentLevel = Level.Normal;
    [SerializeField] GridLayoutGroup _gridLayoutGroup = null;
    [SerializeField, Tooltip("セルのプレハブ")] Cell _cellPrefab = null;
    [SerializeField, Tooltip("経過時間を表示するテキスト")] TMP_Text _timeText = null;
    [SerializeField, Tooltip("ゲーム状態を標準するテキスト")] TMP_Text _playText = null;
    [SerializeField, Tooltip("旗数を表示するテキスト")] TMP_Text _flagCountText = null;
    [SerializeField, Tooltip("難易度を変更するドロップダウン")] TMP_Dropdown _levelDropdown = null;
    int _rows = 10;
    int _columns = 10;
    int _mineCount = 10;
    /// <summary>盤面に置かれている旗数 </summary>
    int _currentFlagCount = 0;
    /// <summary>旗の位置が合っている数 </summary>
    int _rightCount = 0;
    float _gameTime = 0;
    bool _isGame = true;
    bool _isTimerStart = false;
    bool _isfirst = true;
    /// <summary>初めて遊ぶかどうか </summary>
    bool _isFirstGame = true;
    Cell[,] _cells;

    private void OnValidate()
    {
        switch (_currentLevel)
        {
            case Level.Easy:
                _levelDropdown.value = 0;
                break;
            case Level.Normal:
                _levelDropdown.value = 1;
                break;
            case Level.Hard:
                _levelDropdown.value = 2;
                break;
        }
    }

    private void Start()
    {
        ChangeLevel(true);
        CreateBoard();
        SetMine();
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _currentFlagCount = _mineCount;
        _flagCountText.text = _currentFlagCount.ToString();
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
                if (cell.IsFlag) return;

                cell.transform.GetChild(1).GetComponent<Image>().enabled = false;

                if (_isfirst)
                {
                    _isTimerStart = true;
                    _isfirst = false;
                    _playText.text = "プレイ中";
                    if (cell.CellState == CellState.Mine)
                    {
                        ResetMinePos();
                        SetMine(cell);
                    }
                }

                var endFlag = cell.OpenCell();
                if (!endFlag)
                {
                    AroundCellOpen(cell);
                }
                else
                {
                    GameOver(endFlag);
                }


                GameClearCheck();
            }
            else if (Input.GetButtonDown("Fire2") && hit && !_isfirst)  //旗を立てる
            {
                var cell = hit.collider.gameObject.GetComponent<Cell>();
                var Image = cell.transform.GetChild(1).GetComponent<Image>();

                if (cell.IsOpen) return;    //開いていれば何もしない

                if (cell.IsFlag)
                {
                    cell.IsFlag = false;
                    _currentFlagCount++;
                    Image.color = Color.blue;
                }
                else
                {
                    cell.IsFlag = true;
                    _currentFlagCount--;
                    Image.color = Color.red;
                }

                _flagCountText.text = _currentFlagCount.ToString();
                GameClearCheck();
            }

            if (_isTimerStart)  //時間計測を開始する
            {
                _gameTime += Time.deltaTime;
                _timeText.text = _gameTime.ToString("F2");
            }
        }
    }

    /// <summary>
    /// ゲームを再挑戦する
    /// 値をリセットする
    /// </summary>
    public void RetryGame()
    {
        _isfirst = true;
        _isGame = true;
        _isTimerStart = false;
        _isFirstGame = false;
        _currentFlagCount = _mineCount;
        _flagCountText.text = _currentFlagCount.ToString();
        _gameTime = 0;
        _timeText.text = "0.00";
        _playText.text = "準備中";
        CreateBoard();
        SetMine();
    }

    /// <summary>難易度を変更する </summary>
    public void ChangeLevel(bool isFirstGame)
    {
        _currentLevel = (Level)_levelDropdown.value;

        switch (_currentLevel)
        {
            case Level.Easy:
                _rows = _easyData[0];
                _columns = _easyData[1];
                _mineCount = _easyData[2];
                break;
            case Level.Normal:
                _rows = _nomalData[0];
                _columns = _nomalData[1];
                _mineCount = _nomalData[2];
                break;
            case Level.Hard:
                _rows = _hardData[0];
                _columns = _hardData[1];
                _mineCount = _hardData[2];
                break;
        }

        if (!isFirstGame)
        {
            RetryGame();
        }
    }

    /// <summary>盤面を生成する</summary>
    private void CreateBoard()
    {
        if (!_isFirstGame)
        {
            foreach (var cell in _cells)
            {
                Destroy(cell.gameObject);
            }
        }

        _cells = new Cell[_rows, _columns];

        for (var r = 0; r < _rows; r++)
        {
            for (var c = 0; c < _columns; c++)
            {
                var cell = Instantiate(_cellPrefab, _gridLayoutGroup.transform);
                cell.Row = r;
                cell.Column = c;
                _cells[r, c] = cell;
            }
        }

        _gridLayoutGroup.constraintCount = _columns;
    }

    /// <summary>ゲームクリアしているから調べる </summary>
    private void GameClearCheck()
    {
        foreach (var cell in _cells)
        {
            if (cell.CellState == CellState.Mine && cell.IsFlag)
            {
                _rightCount++;
                continue;
            }

            if (cell.IsOpen == false)
            {
                _rightCount = 0;
                return;
            }
        }

        if (_rightCount == _mineCount)
        {
            GameClear();
        }
    }

    /// <summary>時間計測を止める </summary>
    private void GameClear()
    {
        _isTimerStart = false;
        _isGame = false;
        _playText.text = "クリア!";
    }

    /// <summary>地雷の引いたらゲームを終了させる </summary>
    /// <param name="gameover">true=開けたセルが地雷である false=地雷ではない</param>
    private void GameOver(bool gameover)
    {
        if (gameover)
        {
            _playText.text = "失敗...";
            _isGame = false;
        }
    }

    /// <summary>地雷をランダムにセットする </summary>
    private void SetMine()
    {
        var count = Mathf.Min(_mineCount, _cells.Length);

        for (var i = 0; i < count;)
        {
            var r = Random.Range(0, _rows);
            var c = Random.Range(0, _columns);
            var cell = _cells[r, c];

            if (cell.CellState != CellState.Mine)　//選択されたセルの状態が空であれば地雷にする
            {
                i++;
                cell.CellState = CellState.Mine;
                SetMineCount(cell);
            }
        }
    }

    /// <summary>プレイヤーが一手目に地雷を引いたら地雷を再セットする</summary>
    private void SetMine(Cell firstCell)
    {
        var count = Mathf.Min(_mineCount, _cells.Length);

        for (var i = 0; i < count; i++)
        {
            var r = Random.Range(0, _rows);
            var c = Random.Range(0, _columns);
            var cell = _cells[r, c];

            if (cell.CellState == CellState.None && firstCell != cell)
            {
                cell.CellState = CellState.Mine;
                SetMineCount(cell);
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
    private void SetMineCount(Cell target)
    {
        int r = target.Row;
        int c = target.Column;

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
    private void CheckMine(int r, int c)
    {
        var cell = _cells[r, c];
        if (cell.CellState != CellState.Mine)
        {
            cell.MineCount++;
            cell.CellState = CellState.Count;
        }
    }

    /// <summary>
    /// 開いたセルの周囲八マスを開ける 
    /// 更に開いたセルが空だったら周囲八マスのセルを開ける
    /// </summary>
    private void AroundCellOpen(Cell target)
    {
        if (target.CellState == CellState.Count) return;

        var r = target.Row;
        var c = target.Column;

        if (0 <= r - 1) //上
        {
            var cell = _cells[r - 1, c];

            if (!cell.IsOpen)
            {
                cell.OpenCell();
                AroundCellOpen(cell);
            }
        }

        if (0 <= r - 1 && c + 1 < _columns) //右上
        {
            var cell = _cells[r - 1, c + 1];

            if (!cell.IsOpen)
            {
                cell.OpenCell();
                AroundCellOpen(cell);
            }
        }

        if (0 <= c - 1) //左
        {
            var cell = _cells[r, c - 1];

            if (!cell.IsOpen)
            {
                cell.OpenCell();
                AroundCellOpen(cell);
            }
        }

        if (c + 1 < _columns)   //右
        {
            var cell = _cells[r, c + 1];

            if (!cell.IsOpen)
            {
                cell.OpenCell();
                AroundCellOpen(cell);
            }
        }

        if (r + 1 < _rows && 0 <= c - 1) //左下
        {
            var cell = _cells[r + 1, c - 1];

            if (!cell.IsOpen)
            {
                cell.OpenCell();
                AroundCellOpen(cell);
            }
        }

        if (r + 1 < _rows) //下
        {
            var cell = _cells[r + 1, c];

            if (!cell.IsOpen)
            {
                cell.OpenCell();
                AroundCellOpen(cell);
            }
        }

        if (r + 1 < _rows && c + 1 < _columns) //右下
        {
            var cell = _cells[r + 1, c + 1];

            if (!cell.IsOpen)
            {
                cell.OpenCell();
                AroundCellOpen(cell);
            }
        }
    }
}

public enum Level
{
    Easy = 0,
    Normal = 1,
    Hard = 2,
}
