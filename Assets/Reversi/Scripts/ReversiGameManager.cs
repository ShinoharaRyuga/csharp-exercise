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
    [SerializeField] BoardState _turn = BoardState.Black;
    [SerializeField] Material _lightGreen = default;
    List<Stone> _whiteStones = new List<Stone>();
    List<Stone> _blackStones = new List<Stone>();
    /// <summary>石を置くことが出来るマス目のリスト </summary>
    List<Board> _putPoints = new List<Board>();
    /// <summary>0=なし 1=黒 2=白 </summary>
    Board[,] _board = new Board[BOARD_SIZE_X, BOARD_SIZE_Z];

    int count = 0;

    public BoardState Turn { get => _turn; }

    private void Start()
    {
        Setup();

        //初期石を取得しリストに追加する
        var stones = GameObject.FindGameObjectsWithTag("Stone");

        foreach (var stone in stones)
        {
            var playerStone = stone.GetComponent<Stone>();

            if (playerStone.CurrentState == BoardState.White)
            {
                _whiteStones.Add(playerStone);
            }
            else if (playerStone.CurrentState == BoardState.Black)
            {
                _blackStones.Add(playerStone);
            }
        }

        CheckPutStone();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            for (var i = 0; i < 8; i++)
            {
                var str = "";
                for (var k = 0; k < 8; k++)
                {
                    var state = _board[i, k].CurrentState;
                    str += " " + (int)state;

                    if (k == 7)
                    {
                        Debug.Log(str);
                    }
                }
            }
        }
    }
    public void CheckPutStone()
    {
        if (_turn == BoardState.White)
        {
            Debug.Log("white");
            foreach (var whiteStone in _whiteStones)
            {
                if (whiteStone.PutBoard.BoardPosZ + 1 < BOARD_SIZE_Z)   //上
                {
                    if (_board[whiteStone.PutBoard.BoardPosX, whiteStone.PutBoard.BoardPosZ + 1].CurrentState == BoardState.Black)
                    {
                        var up = _board[whiteStone.PutBoard.BoardPosX, whiteStone.PutBoard.BoardPosZ + 1];
                        TryPutUpLine(up);
                    }
                }

                if (0 <= whiteStone.PutBoard.BoardPosZ - 1)  //下
                {
                    if (_board[whiteStone.PutBoard.BoardPosX, whiteStone.PutBoard.BoardPosZ - 1].CurrentState == BoardState.Black)
                    {
                        var down = _board[whiteStone.PutBoard.BoardPosX, whiteStone.PutBoard.BoardPosZ - 1];
                        TryPutDownLine(down);
                    }
                }

                if (whiteStone.PutBoard.BoardPosX + 1 < BOARD_SIZE_X)   //右
                {
                    if (_board[whiteStone.PutBoard.BoardPosX + 1, whiteStone.PutBoard.BoardPosZ].CurrentState == BoardState.Black)
                    {
                        var right = _board[whiteStone.PutBoard.BoardPosX + 1, whiteStone.PutBoard.BoardPosZ];
                        TryPutRightLine(right);
                    }
                }

                if (0 <= whiteStone.PutBoard.BoardPosX - 1)     //左
                {
                    if (_board[whiteStone.PutBoard.BoardPosX - 1, whiteStone.PutBoard.BoardPosZ].CurrentState == BoardState.Black)
                    {
                        var left = _board[whiteStone.PutBoard.BoardPosX - 1, whiteStone.PutBoard.BoardPosZ];
                        TryPutLeftLine(left);
                    }
                }
            }
        }
        else if (_turn == BoardState.Black)
        {
            Debug.Log("black");
            foreach (var blackStone in _blackStones)
            {
                if (blackStone.PutBoard.BoardPosZ + 1 < BOARD_SIZE_Z)   //上
                {
                    if (_board[blackStone.PutBoard.BoardPosX, blackStone.PutBoard.BoardPosZ + 1].CurrentState == BoardState.White)
                    {
                        var up = _board[blackStone.PutBoard.BoardPosX, blackStone.PutBoard.BoardPosZ + 1];
                        TryPutUpLine(up);
                    }
                }

                if (0 <= blackStone.PutBoard.BoardPosZ - 1)  //下
                {
                    if (_board[blackStone.PutBoard.BoardPosX, blackStone.PutBoard.BoardPosZ - 1].CurrentState == BoardState.White)
                    {
                        var down = _board[blackStone.PutBoard.BoardPosX, blackStone.PutBoard.BoardPosZ - 1];
                        TryPutDownLine(down);
                    }
                }

                if (blackStone.PutBoard.BoardPosX + 1 < BOARD_SIZE_X)   //右
                {
                    if (_board[blackStone.PutBoard.BoardPosX + 1, blackStone.PutBoard.BoardPosZ].CurrentState == BoardState.White)
                    {
                        var right = _board[blackStone.PutBoard.BoardPosX + 1, blackStone.PutBoard.BoardPosZ];
                        TryPutRightLine(right);
                    }
                }

                if (0 <= blackStone.PutBoard.BoardPosX - 1)     //左
                {
                    if (_board[blackStone.PutBoard.BoardPosX - 1, blackStone.PutBoard.BoardPosZ].CurrentState == BoardState.White)
                    {
                        var left = _board[blackStone.PutBoard.BoardPosX - 1, blackStone.PutBoard.BoardPosZ];
                        TryPutLeftLine(left);
                    }
                }
            }
        }

        _putPoints.ForEach(b => b.IsPut = true);
        _putPoints.ForEach(b => Debug.Log(b.gameObject.name));
    }

    void TryPutUpLine(Board target)
    {
        if (BOARD_SIZE_Z <= target.BoardPosZ + 1) { return; }
        var tryPutPos = (int)_board[target.BoardPosX, target.BoardPosZ + 1].CurrentState;
        var nextBoard = _board[target.BoardPosX, target.BoardPosZ + 1];

        TryPut(tryPutPos, nextBoard, TryPutUpLine);
    }

    void TryPutDownLine(Board target)
    {
        if (target.BoardPosZ - 1 < 0) { return; }

        var tryPutPos = (int)_board[target.BoardPosX, target.BoardPosZ - 1].CurrentState;
        var nextBoard = _board[target.BoardPosX, target.BoardPosZ - 1];
        TryPut(tryPutPos, nextBoard, TryPutDownLine);
    }

    void TryPutRightLine(Board target)
    {
        if (BOARD_SIZE_X <= target.BoardPosX + 1) { return; }

        var tryPutPos = (int)_board[target.BoardPosX + 1, target.BoardPosZ].CurrentState;
        var nextBoard = _board[target.BoardPosX + 1, target.BoardPosZ];
        TryPut(tryPutPos, nextBoard, TryPutRightLine);
    }

    void TryPutLeftLine(Board target)
    {
        if (target.BoardPosX - 1 < 0) { return; }

        var tryPutPos = (int)_board[target.BoardPosX - 1, target.BoardPosZ].CurrentState;
        var nextBoard = _board[target.BoardPosX - 1, target.BoardPosZ];
        TryPut(tryPutPos, nextBoard, TryPutLeftLine);
    }

    void TryPut(int tryPutPos, Board nextBoard, Action<Board> action)
    {
        if (_turn == BoardState.White)
        {
            switch (tryPutPos)
            {
                case 0:     //石が置かれていない
                    _putPoints.Add(nextBoard);
                    break;
                case 1:     //黒石が置かれている
                    action(nextBoard);
                    break;
            }
        }
        else if (_turn == BoardState.Black)
        {
            switch (tryPutPos)
            {
                case 0:     //石が置かれていない
                    _putPoints.Add(nextBoard);
                    break;
                case 2:     //白石が置かれている
                    action(nextBoard);
                    break;
            }
        }
    }

    /// <summary>指定した位置に石を生成する </summary>
    /// <param name="pos">生成位置</param>
    /// <param name="putBoard">置かれるマス目</param>
    public Stone StoneGenerator(Vector3 pos, Board putBoard)
    {
        count++;
        var stone = Instantiate(_stonePrefab, pos, Quaternion.identity);
        stone.PutBoard = putBoard;
        stone.gameObject.name = count.ToString();

        if (_turn == BoardState.White)
        {
            stone.transform.Rotate(180, 0, 0);
            _board[putBoard.BoardPosX, putBoard.BoardPosZ].CurrentState = BoardState.White;
            stone.CurrentState = BoardState.White;
            _whiteStones.Add(stone);
        }
        else if (_turn == BoardState.Black)
        {
            _board[putBoard.BoardPosX, putBoard.BoardPosZ].CurrentState = BoardState.Black;
            _blackStones.Add(stone);
        }

        _putPoints.ForEach(b => b.IsPut = false);
        _putPoints.Clear();
        ChangeTurn();
        CheckPutStone();
        return stone;
    }

    /// <summary>手番を切り替える </summary>
    public void ChangeTurn()
    {
        if (_turn == BoardState.White)
        {
            _turn = BoardState.Black;
        }
        else if (_turn == BoardState.Black)
        {
            _turn = BoardState.White;
        }
    }

    /// <summary>盤面生成や石の初期位置を設定を行う </summary>
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

                //マス目の色を変更する
                if (z % 2 == 0 && x % 2 == 0 || z % 2 == 1 && x % 2 == 1)
                {
                    board.gameObject.GetComponent<MeshRenderer>().material = _lightGreen;
                }

                //下記処理で初期位置に各石を生成
                if (x == 3 && z == 3 || x == 4 && z == 4)   //白
                {
                    board.OnStone = true;
                    var pos = new Vector3(x, STONE_GENERATE_POS_Y, z);
                    count++;
                    FirstStoneGenerator(BoardState.White, board, pos);

                }

                if (x == 3 && z == 4 || x == 4 && z == 3)   //黒
                {
                    board.OnStone = true;
                    var pos = new Vector3(x, STONE_GENERATE_POS_Y, z);
                    count++;
                    FirstStoneGenerator(BoardState.Black, board, pos);
                }
            }
        }

        //白の初期位置を設定
        _board[4, 4].CurrentState = BoardState.White;
        _board[3, 3].CurrentState = BoardState.White;
        //黒の初期位置を設定
        _board[3, 4].CurrentState = BoardState.Black;
        _board[4, 3].CurrentState = BoardState.Black;
    }

    /// <summary>初期石を生成する</summary>
    void FirstStoneGenerator(BoardState color, Board putBorad, Vector3 generatorPos)
    {
        var stone = Instantiate(_stonePrefab, generatorPos, Quaternion.identity);
        stone.PutBoard = putBorad;
        stone.gameObject.name = count.ToString();

        if (color == BoardState.White)
        {
            stone.transform.Rotate(180, 0, 0);
            stone.CurrentState = BoardState.White;
        }
    }
}
