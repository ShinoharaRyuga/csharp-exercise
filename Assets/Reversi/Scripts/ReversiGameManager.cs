using System;
using System.Collections.Generic;
using UnityEngine;

public class ReversiGameManager : MonoBehaviour
{
    const float STONE_GENERATE_POS_Y = 0.16f;
    const int BOARD_SIZE_X = 8;
    const int BOARD_SIZE_Z = 8;

    [SerializeField] Stone _stonePrefab = default;
    [SerializeField] Board boardPrefab = default;
    [SerializeField] Transform _boardParent = default;
    [SerializeField] StoneColor _turn = StoneColor.Black;
    List<Stone> _whiteStones = new List<Stone>();
    List<Stone> _blackStones = new List<Stone>();
    List<Board> _putPoints = new List<Board>();
    /// <summary>0=なし 1=黒 2=白 </summary>
    int[,] _stonePosArray = new int[BOARD_SIZE_X, BOARD_SIZE_Z];
    Board[,] _board = new Board[BOARD_SIZE_X, BOARD_SIZE_Z];

    public StoneColor Turn { get => _turn; }

    private void Start()
    {
        Setup();

        //debug
        //for (var i = 0; i < 8; i++)
        //{
        //    var str = "";
        //    for (var k = 0; k < 8; k++)
        //    {

        //        str += " " + _board[i, k].ToString();

        //        if (k == 7)
        //        {
        //            Debug.Log(str);
        //        }
        //    }
        //}

        //初期石を取得しリストに追加する
        var stones = GameObject.FindGameObjectsWithTag("Stone");

        foreach (var stone in stones)
        {
            var playerStone = stone.GetComponent<Stone>();

            if (playerStone.CurrentState == StoneColor.White)
            {
                _whiteStones.Add(playerStone);
            }
            else if (playerStone.CurrentState == StoneColor.Black)
            {
                _blackStones.Add(playerStone);
            }
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            CheckPutStone();
        }
    }

    public void CheckPutStone()
    {
        if (_turn == StoneColor.White)
        {
            foreach (var whiteStone in _whiteStones)
            {
                if (whiteStone.PutBoard.BoardPosZ + 1 < BOARD_SIZE_Z)   //上
                {
                    if (_stonePosArray[whiteStone.PutBoard.BoardPosX, whiteStone.PutBoard.BoardPosZ + 1] == 1)
                    {
                        var up = _board[whiteStone.PutBoard.BoardPosX, whiteStone.PutBoard.BoardPosZ + 1];
                        TryPutUpLine(up);
                        Debug.Log("発射");
                    }
                }
            }
        }
        else if (_turn == StoneColor.Black)
        {
            foreach (var blackStone in _blackStones)
            {
                Debug.Log(blackStone);

                if (blackStone.PutBoard.BoardPosZ + 1 < BOARD_SIZE_Z)   //上
                {
                    if (_stonePosArray[blackStone.PutBoard.BoardPosX, blackStone.PutBoard.BoardPosZ + 1] == 2)
                    {
                        var up = _board[blackStone.PutBoard.BoardPosX, blackStone.PutBoard.BoardPosZ + 1];
                        TryPutUpLine(up);
                        Debug.Log("発射");
                    }
                }
            }
        }

        _putPoints.ForEach(b => b.IsPut = true);
    }

    void TryPutUpLine(Board target)
    {
        var tryPutPos = _stonePosArray[target.BoardPosX, target.BoardPosZ + 1];
        var nextBoard = _board[target.BoardPosX, target.BoardPosZ + 1];

        if (_turn == StoneColor.White)
        {
            switch (tryPutPos)
            {
                case 0:     //石が置かれていない
                    _putPoints.Add(nextBoard);
                    break;
                case 1:     //黒石が置かれている
                    TryPutUpLine(nextBoard);
                    break;
            }
        }
        else if (_turn == StoneColor.Black)
        {
            switch (tryPutPos)
            {
                case 0:     //石が置かれていない
                    _putPoints.Add(nextBoard);
                    break;
                case 2:     //白石が置かれている
                    TryPutUpLine(nextBoard);
                    break;
            }
        }
    }

    void CheckBoard()
    {
        //if (0 <= r - 1) //上
        //{

        //}

        //if (0 <= r - 1 && c + 1 < BOARD_SIZE_Z) //右上
        //{

        //}

        //if (0 <= c - 1) //左
        //{

        //}

        //if (c + 1 < BOARD_SIZE_Z)   //右
        //{

        //}

        //if (r + 1 < BOARD_SIZE_X && 0 <= c - 1) //左下
        //{

        //}

        //if (r + 1 < BOARD_SIZE_X) //下
        //{

        //}

        //if (r + 1 < BOARD_SIZE_X && c + 1 < BOARD_SIZE_Z) //右下
        //{

        //}
    }

    /// <summary>指定した位置に石を生成する </summary>
    /// <param name="pos">生成位置</param>
    /// <param name="putBoard">置かれるマス目</param>
    public void StoneGenerator(Vector3 pos, Board putBoard)
    {
        var stone = Instantiate(_stonePrefab, pos, Quaternion.identity);
        stone.PutBoard = putBoard;
        _stonePosArray[putBoard.BoardPosX, putBoard.BoardPosZ] = 1;
        if (_turn == StoneColor.White)
        {
            stone.transform.Rotate(180, 0, 0);
            _stonePosArray[putBoard.BoardPosX, putBoard.BoardPosZ] = 2;
        }

        ChangeTurn();
        _putPoints.Clear();
    }

    /// <summary>手番を切り替える </summary>
    public void ChangeTurn()
    {
        if (_turn == StoneColor.White)
        {
            _turn = StoneColor.Black;
        }
        else if (_turn == StoneColor.Black)
        {
            _turn = StoneColor.White;
        }
    }

    /// <summary>盤面生成や石の初期位置を設定 </summary>
    void Setup()
    {
        //各マスの名前
        var names = new string[] { "a", "b", "c", "d", "e", "f", "g", "h" };

        //盤面を生成
        for (var x = 0; x < BOARD_SIZE_X; x++)
        {
            for (var z = 0; z < BOARD_SIZE_Z; z++)
            {
                var generatePos = new Vector3(x, 0, z);
                var board = Instantiate(boardPrefab, generatePos, Quaternion.identity);
                board.gameObject.name = $"{names[x]}{z + 1}";
                _board[x, z] = board;
                board.transform.parent = _boardParent;
                board.BoardPosX = x;
                board.BoardPosZ = z;

                //下記処理で初期位置に各石を生成
                if (x == 3 && z == 3)
                {
                    board.OnStone = true;
                    var pos = new Vector3(x, STONE_GENERATE_POS_Y, z);
                    FirstStoneGenerator(StoneColor.White, board, pos);
                }

                if (x == 4 && z == 4)
                {
                    board.OnStone = true;
                    var pos = new Vector3(x, STONE_GENERATE_POS_Y, z);
                    FirstStoneGenerator(StoneColor.White, board, pos);
                }

                if (x == 3 && z == 4)
                {
                    board.OnStone = true;
                    var pos = new Vector3(x, STONE_GENERATE_POS_Y, z);
                    FirstStoneGenerator(StoneColor.Black, board, pos);
                }

                if (x == 4 && z == 3)
                {
                    board.OnStone = true;
                    var pos = new Vector3(x, STONE_GENERATE_POS_Y, z);
                    FirstStoneGenerator(StoneColor.Black, board, pos);
                }
            }
        }

        //白の初期位置を設定
        _stonePosArray[4, 4] = 2;
        _stonePosArray[3, 3] = 2;
        //黒の初期位置を設定
        _stonePosArray[3, 4] = 1;
        _stonePosArray[4, 3] = 1;
    }

    /// <summary>初期石を生成する</summary>
    void FirstStoneGenerator(StoneColor color, Board putBorad, Vector3 generatorPos)
    {
        var stone = Instantiate(_stonePrefab, generatorPos, Quaternion.identity);
        stone.PutBoard = putBorad;

        if (color == StoneColor.White)
        {
            stone.transform.Rotate(180, 0, 0);
            stone.CurrentState = StoneColor.White;
        }
    }
}
