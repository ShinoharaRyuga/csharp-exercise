using UnityEngine;
using TMPro;

/// <summary>�v���C���[�̓��͂��󂯎��I�����ꂽ�Z���̏�Ԃ�ύX���� </summary>
public class PlayerInput : MonoBehaviour
{
    [SerializeField, Tooltip("������ɑJ�ڂ���܂ł̎��Ԃ��󂯕t����")] TMP_InputField _inputField = default;
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

        if (Input.GetButtonDown("Fire1") && hit)    //�Z����I������
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

    /// <summary>������ɑJ�ڂ���܂ł̎��Ԃ�ύX���� </summary>
    public void ChangeStepTime()
    {
        var stepTime = float.Parse(_inputField.text);
        _gameManager.StepTime = stepTime;
        _inputField.text = _gameManager.StepTime.ToString();
    }
}
