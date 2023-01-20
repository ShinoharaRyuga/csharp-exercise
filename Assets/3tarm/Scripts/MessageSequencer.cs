using UnityEngine;

public class MessageSequencer : MonoBehaviour
{
    [SerializeField]
     private MessagePrinter _printer = default;

    [SerializeField]
    private string[] _messages = default;

    /// <summary>_messages フィールドから表示する現在のメッセージのインデックス </summary>
    private int _currentIndex = -1;

    private void Start()
    {
        MoveNext();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           
            if (_printer is { IsPrinting: false }) // if (_printer != null && !_printer.IsPrinting) と同じ
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
    /// 次のページに進む。
    /// 次のページが存在しない場合は無視する。
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