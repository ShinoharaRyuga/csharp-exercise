using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>���C�t�Q�[�����Ǘ�����N���X </summary>
public class LifeGameManager : MonoBehaviour
{
    [SerializeField, Tooltip("�c�̒���")] int _rows = 10;
    [SerializeField, Tooltip("���̒���")] int _columns = 10;
    [SerializeField, Tooltip("�ŏ��ɐ����Ă���Z���̐�")] int _randomCellCount = 0;
    [SerializeField, Tooltip("������Ɉڂ�܂Ŏ���")] float _stapTime = 1f;
    [SerializeField, Tooltip("�Z���̃v���n�u")] LifeGameCell _cellPrefab = null;
    [SerializeField, Tooltip("�Q�[���̏�Ԃ�\������e�L�X�g")] TMP_Text _playText = default;
    [SerializeField] GridLayoutGroup _gridLayoutGroup = null;
    LifeGameCell[,] _cells;
    float _time = 0f;
    /// <summary>�v���C�����ǂ��� </summary>
    bool _isPlay = false;

    void Start()
    {
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = _columns;
        _cells = new LifeGameCell[_rows, _columns];

        //�Z���𐶐�����
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

        SetLiveCell();
    }

    private void Update()
    {
        if (_isPlay)
        {
            _time += Time.deltaTime;

            if (_time >= _stapTime) //�w�肳�ꂽ���Ԃ��o�߂����玟����̃Z���ɂ���
            {
                SetNextCellState();
                _time = 0f;
            }
        }
    }

    /// <summary>�ŏ��̐����Ă���Z�������߂�i�����_���j </summary>
    public void SetLiveCell()
    {
        foreach (var cell in _cells)
        {
            cell.State = LifeGameCellState.Die;
            cell.NextState = LifeGameCellState.Die;
        }

        for (var i = 0; i < _randomCellCount;)
        {
            var r = UnityEngine.Random.Range(0, _rows);
            var c = UnityEngine.Random.Range(0, _columns);
            var cell = _cells[r, c];

            if (cell.State != LifeGameCellState.Live)
            {
                cell.State = LifeGameCellState.Live;
                i++;
            }
        }
    }

    /// <summary>������̃Z����Ԃ����߂�</summary>
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

    public void PlayStart()
    {
        _isPlay = true;
        _playText.text = "�Đ���";
    }

    public void PlayStop()
    {
        _isPlay = false;
        _playText.text = "��~��";
    }

    /// <summary>�蓮�Ő����i�߂����Ƃ��v���C���[�ɒm�点�� </summary>
    public void MoveingNextStepText()
    {
        _playText.text = "�ꐢ��i�݂܂���";
    }

    /// <summary>�Z����Ԃ����Z�b�g�������Ƃ�m�点�� </summary>
    public void ResetCellsText()
    {
        _playText.text = "���Z�b�g����܂���";
    }

    /// <summary>
    /// ���͔��}�X�Ő����Ă���Z���̐����擾����
    /// </summary>
    /// <param name="targetCell">�w�肳�ꂽ�Z���̈ʒu</param>
    /// <returns></returns>
    private int GetAroubdCellInfo(LifeGameCell targetCell)
    {
        var r = targetCell.Row;
        var c = targetCell.Col;
        var liveCount = 0;

        if (0 <= r - 1 && 0 <= c - 1)   //����
        {
            liveCount += AddCount(r - 1, c - 1);
        }

        if (0 <= r - 1) //��
        {
            liveCount += AddCount(r - 1, c);
        }

        if (0 <= r - 1 && c + 1 < _columns) //�E��
        {
            liveCount += AddCount(r - 1, c + 1);
        }

        if (0 <= c - 1) //��
        {
            liveCount += AddCount(r, c - 1);
        }

        if (c + 1 < _columns)   //�E
        {
            liveCount += AddCount(r, c + 1);
        }


        if (r + 1 < _rows && 0 <= c - 1) //����
        {
            liveCount += AddCount(r + 1, c - 1);
        }

        if (r + 1 < _rows) //��
        {
            liveCount += AddCount(r + 1, c);
        }

        if (r + 1 < _rows && c + 1 < _columns) //�E��
        {
            liveCount += AddCount(r + 1, c + 1);
        }

        return liveCount;
    }

    /// <summary>���ׂ��Z���������Ă����琔�𑝂₷ </summary>
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
