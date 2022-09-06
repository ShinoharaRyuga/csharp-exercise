using UnityEngine;

public class ReversiGameManager : MonoBehaviour
{
    [SerializeField] Stone _stonePrefab = default;
    StoneColor _turn = StoneColor.Black;

    public StoneColor Turn { get => _turn; }

    public void StoneGenerator(Vector3 pos)
    {
        var stone = Instantiate(_stonePrefab, pos, Quaternion.identity);

        if (_turn == StoneColor.White)
        {
            stone.transform.Rotate(180, 0, 0);
        }

        ChangeTurn();
    }

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
}
