using UnityEngine;
using UnityEngine.UI;

/// <summary> ���C�t�Q�[���Ŏg�p����Z�����Ǘ�����N���X </summary>
public class LifeGameCell : MonoBehaviour
{
    [SerializeField, Tooltip("���݂̏��")] LifeGameCellState _currnetState = LifeGameCellState.Die;
    int _row = 0;
    int _col = 0;
    /// <summary>������̃Z�����</summary>
    LifeGameCellState _nextState = LifeGameCellState.Die;
    Image _cellImage => GetComponent<Image>();

    public LifeGameCellState State 
    {
        get { return _currnetState; }
        set
        {
            _currnetState = value;
            ChangeColor();
        }
    }

    /// <summary>�Z���������Ă��� </summary>
    public bool IsLive => State == LifeGameCellState.Live;
    public int Row { get => _row; set => _row = value; }
    public int Col { get => _col; set => _col = value; }
    public LifeGameCellState NextState { get => _nextState; set => _nextState = value; }

    private void OnValidate()
    {
        ChangeColor();
    }

    /// <summary>������̃Z����Ԃ����߂� </summary>
    /// <param name="aliveCount">���͔��}�X�Ő����Ă���Z���̐�</param>
    public void SetNextCellState(int aliveCount)
    {
        if (!IsLive)   
        {
            if (aliveCount == 3) //�a��
            {
                _nextState = LifeGameCellState.Live;
            }
        }
        else
        {
            if (aliveCount == 2 || aliveCount == 3) //����
            {
                _nextState= LifeGameCellState.Live;
            }

            if (aliveCount <= 1)   //�ߑa
            {
                _nextState = LifeGameCellState.Die;
            }

            if (4 <= aliveCount)   //�ߖ�
            {
                _nextState = LifeGameCellState.Die;
            }
        }
    }

    /// <summary>���݂̃Z����Ԃ�������̃Z����Ԃɂ��� </summary>
    public void ChangeStep()
    {
        _currnetState = _nextState;
        ChangeColor();
    }

    /// <summary>��Ԃɂ���ĐF��ς���</summary>
    void ChangeColor()
    {
        switch(_currnetState)
        {
            case LifeGameCellState.Live:
                _cellImage.color = Color.green;
                break;
            case LifeGameCellState.Die:
                _cellImage.color = Color.white;
                break;
        }
    }
}

public enum LifeGameCellState
{
    Live,
    Die
}
