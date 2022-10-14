using TMPro;
using UnityEngine;

/// <summary>パターンを作成するクラス </summary>
public class CreatePattern : MonoBehaviour
{
    [SerializeField, Tooltip("進行状況を表示するテキスト")] TMP_Text _playText = default;
    LifeGameManager _gameManager => GetComponent<LifeGameManager>();

    public void Pulsar()
    {
        if(_gameManager.Rows != 15 && _gameManager.Columns != 32) { return; }

        _gameManager.ResetCellState();
        _playText.text = "パルサー";

        //左上
        ChangeCellStateLive(6, 9);
        ChangeCellStateLive(6, 10);
        ChangeCellStateLive(6, 11);
        ChangeCellStateLive(5, 12);
        ChangeCellStateLive(4, 12);
        ChangeCellStateLive(3, 12);
        ChangeCellStateLive(1, 9);
        ChangeCellStateLive(1, 10);
        ChangeCellStateLive(1, 11);
        ChangeCellStateLive(3, 7);
        ChangeCellStateLive(4, 7);

        //左下
        ChangeCellStateLive(5, 7);
        ChangeCellStateLive(8, 9);
        ChangeCellStateLive(8, 10);
        ChangeCellStateLive(8, 11);
        ChangeCellStateLive(9, 12);
        ChangeCellStateLive(10, 12);
        ChangeCellStateLive(11, 12);
        ChangeCellStateLive(13, 9);
        ChangeCellStateLive(13, 10);
        ChangeCellStateLive(13, 11);
        ChangeCellStateLive(9, 7);
        ChangeCellStateLive(10, 7);
        ChangeCellStateLive(11, 7);

        //右上
        ChangeCellStateLive(3, 14);
        ChangeCellStateLive(4, 14);
        ChangeCellStateLive(5, 14);
        ChangeCellStateLive(6, 15);
        ChangeCellStateLive(6, 16);
        ChangeCellStateLive(6, 17);
        ChangeCellStateLive(3, 19);
        ChangeCellStateLive(4, 19);
        ChangeCellStateLive(5, 19);
        ChangeCellStateLive(1, 15);
        ChangeCellStateLive(1, 16);
        ChangeCellStateLive(1, 17);

        //右下
        ChangeCellStateLive(9, 14);
        ChangeCellStateLive(10, 14);
        ChangeCellStateLive(11, 14);
        ChangeCellStateLive(8, 15);
        ChangeCellStateLive(8, 16);
        ChangeCellStateLive(8, 17);
        ChangeCellStateLive(13, 15);
        ChangeCellStateLive(13, 16);
        ChangeCellStateLive(13, 17);
        ChangeCellStateLive(9, 19);
        ChangeCellStateLive(10, 19);
        ChangeCellStateLive(11, 19);
    }

    public void Octagon()
    {
        if (_gameManager.Rows != 15 && _gameManager.Columns != 32) { return; }

        _gameManager.ResetCellState();
        _playText.text = "八角形";

        //左上
        ChangeCellStateLive(5, 11);
        ChangeCellStateLive(4, 10);
        ChangeCellStateLive(3, 11);
        ChangeCellStateLive(4, 12);

        //左下
        ChangeCellStateLive(6, 11);
        ChangeCellStateLive(7, 10);
        ChangeCellStateLive(8, 11);
        ChangeCellStateLive(7, 12);

        //右上
        ChangeCellStateLive(4, 13);
        ChangeCellStateLive(3, 14);
        ChangeCellStateLive(4, 15);
        ChangeCellStateLive(5, 14);

        //右下
        ChangeCellStateLive(6, 14);
        ChangeCellStateLive(7, 13);
        ChangeCellStateLive(8, 14);
        ChangeCellStateLive(7, 15);
    }

    public void Galaxy()
    {
        if (_gameManager.Rows != 15 && _gameManager.Columns != 32) { return; }

        _gameManager.ResetCellState();
        _playText.text = "銀河";

        //左
        for (var c = 10; c <= 11; c++)
        {
            for (var r = 6; r <= 11; r++)
            {
                ChangeCellStateLive(r, c);
            }
        }

        //下
        for (var r = 10; r <= 11; r++)
        {
            for (var c = 13; c <= 18; c++)
            {
                ChangeCellStateLive(r, c);
            }
        }

        //右
        for (var c = 17; c <= 18; c++)
        {
            for (var r = 3; r <= 8; r++)
            {
                ChangeCellStateLive(r, c);
            }
        }

        //上
        for (var r = 3; r <= 4; r++)
        {
            for (var c = 10; c <= 15; c++)
            {
                ChangeCellStateLive(r, c);
            }
        }
    }

    public void Pentadecathlon()
    {
        if (_gameManager.Rows != 15 && _gameManager.Columns != 32) { return; }

        _gameManager.ResetCellState();
        _playText.text = "ペンタデカスロン";

        //左
        ChangeCellStateLive(7, 10);
        ChangeCellStateLive(7, 11);
        ChangeCellStateLive(6, 11);
        ChangeCellStateLive(8, 11);

        //右上
        ChangeCellStateLive(7, 16);
        ChangeCellStateLive(6, 16);
        ChangeCellStateLive(7, 17);
        ChangeCellStateLive(8, 16);
    }

    public void DieHard()
    {
        if (_gameManager.Rows != 15 && _gameManager.Columns != 32) { return; }

        _gameManager.ResetCellState();
        _playText.text = "ダイハード";

        //左
        ChangeCellStateLive(5, 9);
        ChangeCellStateLive(5, 10);
        ChangeCellStateLive(6, 10);

        //右
        ChangeCellStateLive(6, 14);
        ChangeCellStateLive(6, 15);
        ChangeCellStateLive(6, 16);
        ChangeCellStateLive(4, 15);
    }

    public void Acorn()
    {
        if (_gameManager.Rows != 15 && _gameManager.Columns != 32) { return; }

        _gameManager.ResetCellState();
        _playText.text = "どんぐり";

        //左
        ChangeCellStateLive(9, 8);
        ChangeCellStateLive(9, 9);
        ChangeCellStateLive(7, 9);

        //右
        ChangeCellStateLive(8, 11);
        ChangeCellStateLive(9, 12);
        ChangeCellStateLive(9, 13);
        ChangeCellStateLive(9, 14);
    }

    public void Glider()
    {
        if (_gameManager.Rows != 15 && _gameManager.Columns != 32) { return; }

        _gameManager.ResetCellState();
        _playText.text = "グライダー";

        ChangeCellStateLive(14, 28);
        ChangeCellStateLive(13, 27);
        ChangeCellStateLive(12, 27);
        ChangeCellStateLive(12, 28);
        ChangeCellStateLive(12, 29);

        ChangeCellStateLive(14, 20);
        ChangeCellStateLive(13, 19);
        ChangeCellStateLive(12, 19);
        ChangeCellStateLive(12, 20);
        ChangeCellStateLive(12, 21);

        ChangeCellStateLive(14, 11);
        ChangeCellStateLive(13, 10);
        ChangeCellStateLive(12, 10);
        ChangeCellStateLive(12, 11);
        ChangeCellStateLive(12, 12);
    }

    public void SpaceShip()
    {
        if (_gameManager.Rows != 15 && _gameManager.Columns != 32) { return; }

        _gameManager.ResetCellState();
        _playText.text = "宇宙船";

        ChangeCellStateLive(5, 27);
        ChangeCellStateLive(6, 26);
        ChangeCellStateLive(7, 26);
        ChangeCellStateLive(8, 26);
        ChangeCellStateLive(8, 27);
        ChangeCellStateLive(8, 28);
        ChangeCellStateLive(8, 29);
        ChangeCellStateLive(7, 30);
        ChangeCellStateLive(5, 30);
    }

    /// <summary>セル状態を生存に変更する </summary>
    void ChangeCellStateLive(int row, int column)
    {
        _gameManager.Cells[row, column].State = LifeGameCellState.Alive;
    }
}
