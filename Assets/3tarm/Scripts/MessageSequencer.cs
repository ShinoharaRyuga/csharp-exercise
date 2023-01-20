using UnityEngine;

public class MessageSequencer : MonoBehaviour
{
    [SerializeField]
     private MessagePrinter _printer = default;

    [SerializeField]
    private string[] _messages = default;

    /// <summary>_messages �t�B�[���h����\�����錻�݂̃��b�Z�[�W�̃C���f�b�N�X </summary>
    private int _currentIndex = -1;

    private void Start()
    {
        MoveNext();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           
            if (_printer is { IsPrinting: false }) // if (_printer != null && !_printer.IsPrinting) �Ɠ���
            {
                MoveNext();
            }
            else
            {
                _printer.Skip();
            }
        }
    }

    /// <summary>
    /// ���̃y�[�W�ɐi�ށB
    /// ���̃y�[�W�����݂��Ȃ��ꍇ�͖�������B
    /// </summary>
    private void MoveNext()
    {
        if (_messages is null or { Length: 0 }) { return; }

        if (_currentIndex + 1 < _messages.Length)
        {
            _currentIndex++;
            _printer?.ShowMessage(_messages[_currentIndex]);
        }
    }
}