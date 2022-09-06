using UnityEngine;

public class ReversiPlayerInput : MonoBehaviour
{
    ReversiGameManager _gameManager => GetComponent<ReversiGameManager>();

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetButtonDown("Fire1") && Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.gameObject.TryGetComponent(out Board board))
            {
                if (!board.OnStone)
                {
                    var generatePos = new Vector3(board.transform.position.x, 0.12f, board.transform.position.z);
                    _gameManager.StoneGenerator(generatePos);
                }
      
                board.OnStone = true;
            }
        }
    }
}
