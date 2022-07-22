using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �Q�[���}�l�W���[ 
/// �v���C���[�̓��͂��󂯎��
/// �Ֆʂ𐶐�����
/// </summary>
public class MineSweeper : MonoBehaviour
{
    [SerializeField, Tooltip("�c�̒���")] int _rows = 10;
    [SerializeField, Tooltip("���̒���")] int _columns = 10;
    [SerializeField, Tooltip("�n���̐�")] int _mineCount = 10;
    [SerializeField, Tooltip("�Z���̏�Ԃ����邱�Ƃ��o����")] ViewMode _viewMode = ViewMode.Game;
    [SerializeField] GridLayoutGroup _gridLayoutGroup = null;
    [SerializeField, Tooltip("�Z���̃v���n�u")] Cell _cellPrefab = null;
    [SerializeField, Tooltip("�o�ߎ��Ԃ�\������e�L�X�g")] Text _timeText = null;
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

        //�Ֆʂ𐶐�����
        for (var r = 0; r < _rows; r++)
        {
            for (var c = 0; c < _columns; c++)
            {
                var cell = Instantiate(_cellPrefab, _gridLayoutGroup.transform);
                cell.SetView(_viewMode);
                cell.Row = r;
                cell.Column = c;
                _cells[r, c] = cell;
            }
        }

        SetMine();
    }

    private void Update()
    {
        foreach (var cell in _cells)    //�n���̈ʒu��������悤�ɂ���@�f�o�b�O�p
        {
            cell.SetView(_viewMode);
        }

        var screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

        if (_isGame)
        {
            if (Input.GetButtonDown("Fire1") && hit)   //�Z�����J����
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
            else if (Input.GetButtonDown("Fire2") && hit)  //���𗧂Ă�
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

            if (_isTimerStart)  //���Ԍv�����J�n����
            {
                _gameTime += Time.deltaTime;
                _timeText.text = _gameTime.ToString("F2");
            }
        }


        if (Input.GetKeyDown(KeyCode.C))    //���g���C�@�f�o�b�O�p
        {
            ResetMinePos();
            SetMine();
        }
    }

    /// <summary>�Q�[���N���A���Ă��邩�璲�ׂ� </summary>
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

    /// <summary>���Ԍv�����~�߂� </summary>
    private void GameClear()
    {
        _isTimerStart = false;
        _isGame = false;
        Debug.Log("�N���A");
    }

    /// <summary>�n���̈�������Q�[�����I�������� </summary>
    /// <param name="gameover">true=�J�����Z�����n���ł��� false=�n���ł͂Ȃ�</param>
    private void GameOver(bool gameover)
    {
        if (gameover)
        {
            Debug.Log("���s");
            _isGame = false;
        }
    }

    /// <summary>�n���������_���ɃZ�b�g���� </summary>
    private void SetMine()
    {
        var count = Mathf.Min(_mineCount, _cells.Length);

        for (var i = 0; i < count;)
        {
            var r = UnityEngine.Random.Range(0, _rows);
            var c = UnityEngine.Random.Range(0, _columns);
            var cell = _cells[r, c];

            if (cell.CellState != CellState.Mine)�@//�I�����ꂽ�Z���̏�Ԃ���ł���Βn���ɂ���
            {
                i++;
                cell.CellState = CellState.Mine;
                SetMineCount(cell);
            }
        }
    }

    /// <summary>�v���C���[�����ڂɒn������������n�����ăZ�b�g����</summary>
    private void SetMine(Cell firstCell)
    {
        var count = Mathf.Min(_mineCount, _cells.Length);

        for (var i = 0; i < count; i++)
        {
            var r = UnityEngine.Random.Range(0, _rows);
            var c = UnityEngine.Random.Range(0, _columns);
            var cell = _cells[r, c];

            if (cell.CellState == CellState.None && firstCell != cell)
            {
                cell.CellState = CellState.Mine;
                SetMineCount(cell);
            }
        }
    }

    /// <summary>�Z���̏�Ԃ���̏�Ԃɂ���</summary>
    private void ResetMinePos()
    {
        foreach (var cell in _cells)
        {
            cell.ResetState();
        }
    }

    /// <summary>�n���̐��𐔂��� </summary>
    /// <param name="r"></param>
    /// <param name="c"></param>
    private void SetMineCount(Cell target)
    {
        int r = target.Row;
        int c = target.Column;

        if (0 <= r - 1 && 0 <= c - 1)   //����
        {
            CheckMine(r - 1, c - 1);
        }

        if (0 <= r - 1) //��
        {
            CheckMine(r - 1, c);
        }

        if (0 <= r - 1 && c + 1 < _columns) //�E��
        {
            CheckMine(r - 1, c + 1);
        }

        if (0 <= c - 1) //��
        {
            CheckMine(r, c - 1);
        }

        if (c + 1 < _columns)   //�E
        {
            CheckMine(r, c + 1);
        }


        if (r + 1 < _rows && 0 <= c - 1) //����
        {
            CheckMine(r + 1, c - 1);
        }

        if (r + 1 < _rows) //��
        {
            CheckMine(r + 1, c);
        }

        if (r + 1 < _rows && c + 1 < _columns) //�E��
        {
            CheckMine(r + 1, c + 1);
        }
    }

    /// <summary>�������g���Z�����擾���̂��̃Z����Ԃ��n���ł͂Ȃ���Ύ��͂̒n���̐���\������ɂ���</summary>
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

    /// <summary>
    /// �J�����Z���̎��͔��}�X���J���� 
    /// �X�ɊJ�����Z�����󂾂�������͔��}�X�̃Z�����J����
    /// �ċN�Ăт���
    /// </summary>
    /// <param name="target"></param>
    private void AroundCellOpen(Cell target)
    {
        if (target.CellState == CellState.Count) return;

        var r = target.Row;
        var c = target.Column;

        if (0 <= r - 1) //��
        {
            var cell = _cells[r - 1, c];

            if (!cell.IsOpen)
            {
                cell.OpenCell();
                AroundCellOpen(cell);
            }
        }

        if (0 <= r - 1 && c + 1 < _columns) //�E��
        {
            var cell = _cells[r - 1, c + 1];

            if (!cell.IsOpen)
            {
                cell.OpenCell();
                AroundCellOpen(cell);
            }
        }

        if (0 <= c - 1) //��
        {
            var cell = _cells[r, c - 1];

            if (!cell.IsOpen)
            {
                cell.OpenCell();
                AroundCellOpen(cell);
            }
        }

        if (c + 1 < _columns)   //�E
        {
            var cell = _cells[r, c + 1];

            if (!cell.IsOpen)
            {
                cell.OpenCell();
                AroundCellOpen(cell);
            }
        }

        if (r + 1 < _rows && 0 <= c - 1) //����
        {
            var cell = _cells[r + 1, c - 1];

            if (!cell.IsOpen)
            {
                cell.OpenCell();
                AroundCellOpen(cell);
            }
        }

        if (r + 1 < _rows) //��
        {
            var cell = _cells[r + 1, c];

            if (!cell.IsOpen)
            {
                cell.OpenCell();
                AroundCellOpen(cell);
            }
        }

        if (r + 1 < _rows && c + 1 < _columns) //�E��
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

public enum ViewMode
{
    All = 0,
    MineOnly = 1,
    Game = 2,
}
