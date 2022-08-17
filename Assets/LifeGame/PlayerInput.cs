using UnityEngine;
using TMPro;

/// <summary>プレイヤーの入力を受け取り選択されたセルの状態を変更する </summary>
public class PlayerInput : MonoBehaviour
{
    [SerializeField, Tooltip("次世代に遷移するまでの時間を受け付ける")] TMP_InputField _inputField = default;
    LifeGameManager _gameManager => GetComponent<LifeGameManager>();

    private void Start()
    {
        _inputField.text = _gameManager.StepTime.ToString();
    }

    void Update()
    {
        var screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

        if (Input.GetButtonDown("Fire1") && hit)    //セルを選択する
        {
            var cell = hit.collider.GetComponent<LifeGameCell>();

            if (cell.State == LifeGameCellState.Live)
            {
                cell.State = LifeGameCellState.Die;
            }
            else
            {
                cell.State = LifeGameCellState.Live;
            }

            Debug.Log($"r{cell.Row} c{cell.Col}");
        }
    }

    /// <summary>次世代に遷移するまでの時間を変更する </summary>
    public void ChangeStepTime()
    {
        var stepTime = float.Parse(_inputField.text);
        _gameManager.StepTime = stepTime;
        _inputField.text = _gameManager.StepTime.ToString();
    }
}
