using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Task3 : MonoBehaviour, IPointerClickHandler
{
    [SerializeField, Tooltip("縦列のセル数")] int _row = 5;
    [SerializeField, Tooltip("横列のセル数")] int _column = 5;
    [SerializeField, Range(0, 9), Tooltip("最初の色を黒にする確率")] int _colorRatio = 0;
    GameObject[,] _cells = default;

    bool _isClear = false;
    bool _isStart = false;
    int _playCount = 0;
    float _clearTime = 0;

    private void Start()
    {
        _cells = new GameObject[_row, _column];
        var layout = GetComponent<GridLayoutGroup>();
        layout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        layout.constraintCount = _column;

        for (var r = 0; r < _row; r++)
        {
            for (var c = 0; c < _column; c++)
            {
                var cell = new GameObject($"Cell({r}, {c})");
                cell.transform.parent = transform;
                cell.AddComponent<CellInformation>().SetPosition(r, c);
                _cells[r, c] = cell;
                var image = cell.AddComponent<Image>();
                var info = cell.GetComponent<CellInformation>();
                FirstCellColor(image, info);
            }
        }
    }

    void Update()
    {
        if (_isStart && !_isClear)
        {
            _clearTime += Time.deltaTime;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isStart)　//一手目だったら時間計測を始める
        {
            _isStart = true;
        }

        var cell = eventData.pointerCurrentRaycast.gameObject;
        var cellInfo = cell.GetComponent<CellInformation>();
        var image = cell.GetComponent<Image>();
        _playCount++;

        if (cellInfo.Color) //押されたセルの色を反転する
        {

            image.color = Color.black;
            cellInfo.Color = false;
        }
        else
        {
            image.color = Color.white;
            cellInfo.Color = true;
        }

        ChangeSellColor(cellInfo);
        CheckGamecClear();
    }

    //押されたセルの上下左右のセルを取得して色を反転させる
    private void ChangeSellColor(CellInformation info)
    {
        if (0 < info.Row) //上
        {
            var cellInfo = _cells[info.Row - 1, info.Column].GetComponent<CellInformation>();

            if (cellInfo.Color)
            {
                cellInfo.gameObject.GetComponent<Image>().color = Color.black;
                cellInfo.Color = false;
            }
            else
            {
                cellInfo.gameObject.GetComponent<Image>().color = Color.white;
                cellInfo.Color = true;
            }
        }

        if (info.Row < _row - 1) //下
        {
            var cellInfo = _cells[info.Row + 1, info.Column].GetComponent<CellInformation>();

            if (cellInfo.Color)
            {
                cellInfo.gameObject.GetComponent<Image>().color = Color.black;
                cellInfo.Color = false;
            }
            else
            {
                cellInfo.gameObject.GetComponent<Image>().color = Color.white;
                cellInfo.Color = true;
            }
        }

        if (info.Column < _column - 1) //右
        {
            var cellInfo = _cells[info.Row, info.Column + 1].GetComponent<CellInformation>();

            if (cellInfo.Color)
            {
                cellInfo.gameObject.GetComponent<Image>().color = Color.black;
                cellInfo.Color = false;
            }
            else
            {
                cellInfo.gameObject.GetComponent<Image>().color = Color.white;
                cellInfo.Color = true;
            }
        }

        if (0 < info.Column) //左
        {
            var cellInfo = _cells[info.Row, info.Column - 1].GetComponent<CellInformation>();

            if (cellInfo.Color)
            {
                cellInfo.gameObject.GetComponent<Image>().color = Color.black;
                cellInfo.Color = false;
            }
            else
            {
                cellInfo.gameObject.GetComponent<Image>().color = Color.white;
                cellInfo.Color = true;
            }
        }
    }

    /// <summary>クリアをしているか確認する </summary>
    private void CheckGamecClear()
    {
        var _clearFlag = true;
        foreach (var cell in _cells)
        {
            var info = cell.GetComponent<CellInformation>();
            if (!info.Color)
            {
                _clearFlag = false;
                break;
            }
        }

        if (_clearFlag)
        {
            _isClear = true;
            Debug.Log("クリア");
            Debug.Log($"クリアタイム{_clearTime}");
            Debug.Log($"手数{_playCount}");
        }
    }

    /// <summary>最初のセルの色を決める とりあえずランダムにする</summary>
    private void FirstCellColor(Image image, CellInformation information)
    {
        var n = Random.Range(0, 10);

        if (n <= _colorRatio)
        {
            image.color = Color.black;
            information.Color = false;
        }
    }
}
