using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>ライフゲームを管理するクラス </summary>
public class LifeGameManager : MonoBehaviour
{
    [SerializeField, Tooltip("縦の長さ")] int _rows = 10;
    [SerializeField, Tooltip("横の長さ")] int _columns = 10;
    [SerializeField, Range(1, 480), Tooltip("ランダム生成時のセル最小数")] int _minCellCount = 1;
    [SerializeField, Range(1, 480), Tooltip("ランダム生成時のセル最大数")] int _maxCellCount = 1;
    [SerializeField, Tooltip("次世代に移るまで時間")] float _stepTime = 1f;
    [SerializeField, Tooltip("セルのプレハブ")] LifeGameCell _cellPrefab = null;
    [SerializeField, Tooltip("ゲームの状態を表示するテキスト")] TMP_Text _playText = default;
    [SerializeField] GridLayoutGroup _gridLayoutGroup = null;
    LifeGameCell[,] _cells;
    float _time = 0f;
    /// <summary>プレイ中かどうか </summary>
    bool _isPlay = false;

    public float StepTime
    {
        get { return _stepTime; }

        set
        {
            if (0 < value)
            {
                _stepTime = value;
            }
        }
    }

    public LifeGameCell[,] Cells { get => _cells; set => _cells = value; }
    public bool IsPlay { get => _isPlay; set => _isPlay = value; }

    void Start()
    {
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = _columns;
        _cells = new LifeGameCell[_rows, _columns];

        //セルを生成する
        for (var r = 0; r < _rows; r++)
        {
            for (var c = 0; c < _columns; c++)
            {
                var cell = Instantiate(_cellPrefab, _gridLayoutGroup.transform);
                cell.Row = r;
                cell.Col = c;
                _cells[r, c] = cell;
            }
        }
    }

    private void Update()
    {
        if (_isPlay)
        {
            _time += Time.deltaTime;

            if (_time >= _stepTime) //指定された時間が経過したら次世代のセルにする
            {
                SetNextCellState();
                _time = 0f;
            }
        }
    }

    /// <summary>セルの状態を初期化する </summary>
    public void ResetCellState()
    {
        _isPlay = false;

        foreach (var cell in _cells) 
        {
            cell.State = LifeGameCellState.Dead;
            cell.NextState = LifeGameCellState.Dead;
        }
    }

    /// <summary>最初の生きているセルを決める（ランダム） </summary>
    public void SetLiveCell()
    {
        ResetCellState();

        var count = Random.Range(_minCellCount, _maxCellCount);

        for (var i = 0; i < count;)
        {
            var r = Random.Range(0, _rows);
            var c = Random.Range(0, _columns);
            var cell = _cells[r, c];

            if (cell.State != LifeGameCellState.Alive)
            {
                cell.State = LifeGameCellState.Alive;
                i++;
            }
        }
    }

    /// <summary>次世代のセル状態を決める</summary>
    public void SetNextCellState()
    {
        foreach (var cell in _cells)
        {
            var aliveCount = GetAroubdCellInfo(cell);
            cell.SetNextCellState(aliveCount);
        }

        foreach (var cell in _cells)
        {
            cell.ChangeStep();
        }
    }

    public void RandomGenerate()
    {
        _playText.text = "ランダム生成しました";
    }

    public void PlayStart()
    {
        _isPlay = true;
        _playText.text = "再生中";
    }

    public void PlayStop()
    {
        _isPlay = false;
        _playText.text = "停止中";
    }

    /// <summary>手動で世代を進めたことをプレイヤーに知らせる </summary>
    public void MoveingNextStepText()
    {
        _playText.text = "一世代進みました";
    }

    /// <summary>セル状態をリセットしたことを知らせる </summary>
    public void ResetCellsText()
    {
        _playText.text = "リセットされました";
    }

    /// <summary>
    /// 周囲八マスで生きているセルの数を取得する
    /// </summary>
    /// <param name="targetCell">指定されたセルの位置</param>
    /// <returns></returns>
    private int GetAroubdCellInfo(LifeGameCell targetCell)
    {
        var r = targetCell.Row;
        var c = targetCell.Col;
        var liveCount = 0;

        if (0 <= r - 1 && 0 <= c - 1)   //左上
        {
            liveCount += AddCount(r - 1, c - 1);
        }

        if (0 <= r - 1) //上
        {
            liveCount += AddCount(r - 1, c);
        }

        if (0 <= r - 1 && c + 1 < _columns) //右上
        {
            liveCount += AddCount(r - 1, c + 1);
        }

        if (0 <= c - 1) //左
        {
            liveCount += AddCount(r, c - 1);
        }

        if (c + 1 < _columns)   //右
        {
            liveCount += AddCount(r, c + 1);
        }


        if (r + 1 < _rows && 0 <= c - 1) //左下
        {
            liveCount += AddCount(r + 1, c - 1);
        }

        if (r + 1 < _rows) //下
        {
            liveCount += AddCount(r + 1, c);
        }

        if (r + 1 < _rows && c + 1 < _columns) //右下
        {
            liveCount += AddCount(r + 1, c + 1);
        }

        return liveCount;
    }

    /// <summary>調べたセルが生きていたら数を増やす </summary>
    private int AddCount(int r, int c)
    {
        var cell = _cells[r, c];

        if (cell.IsLive)
        {
            return 1;
        }

        return 0;
    }
}
