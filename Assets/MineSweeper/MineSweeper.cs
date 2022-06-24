using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MineSweeper : MonoBehaviour
{
    [SerializeField] int _rows = 10;
    [SerializeField] int _columns = 10;
    [SerializeField] int _mineCount = 10;
    [SerializeField] bool _isView = false;
    [SerializeField] GridLayoutGroup _gridLayoutGroup = null;
    [SerializeField] Cell _cellPrefab = null;

    Cell[,] _cells;
    private void Start()
    {
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = _columns;
        _cells = new Cell[_rows, _columns];

        for (var r = 0; r < _rows; r++)
        {
            for (var c = 0; c < _columns; c++)
            {
                var cell = Instantiate(_cellPrefab, _gridLayoutGroup.transform);
                cell.SetView(_isView);
                _cells[r, c] = cell;
            }
        }
      
        for (var i = 0; i < _mineCount; i++)
        {
            var r = Random.Range(0, _rows);
            var c = Random.Range(0, _columns);
            var cell = _cells[r, c];

            if (cell.CellState == CellState.None)
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

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            if (hit)
            {
                Debug.Log(hit.collider.name);
            }
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            if (hit)
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }

    private void SetMineCount(int r, int c)
    {
        if (0 <= r - 1 && 0 <= c - 1)   //¶ã
        {
            var cell = _cells[r - 1, c - 1];
            cell.MineCount++;
            cell.CellState = CellState.Count;
        }

        if (0 <= r - 1) //ã
        {
            var cell = _cells[r - 1, c];
            cell.MineCount++;
            cell.CellState = CellState.Count;
        }

        if (0 <= r - 1 && c + 1 < _columns) //‰Eã
        {
            var cell = _cells[r - 1, c + 1];
            cell.MineCount++;
            cell.CellState = CellState.Count;
        }

        if (0 <= c - 1) //¶
        {
            var cell = _cells[r, c - 1];
            cell.MineCount++;
            cell.CellState = CellState.Count;
        }

        if (c + 1 < _columns)   //‰E
        {
            var cell = _cells[r, c + 1];
            cell.MineCount++;
            cell.CellState = CellState.Count;
        }


        if (r + 1 < _rows && 0 <= c - 1) //¶‰º
        {
            var cell = _cells[r + 1, c - 1];
            cell.MineCount++;
            cell.CellState = CellState.Count;
        }

        if (r + 1 < _rows) //‰º
        {
            var cell = _cells[r + 1, c];
            cell.MineCount++;
            cell.CellState = CellState.Count;
        }

        if (r + 1 < _rows &&  c + 1 < _columns) //‰E‰º
        {
            var cell = _cells[r + 1, c + 1];
            cell.MineCount++;
            cell.CellState = CellState.Count;
        }
    }
}
