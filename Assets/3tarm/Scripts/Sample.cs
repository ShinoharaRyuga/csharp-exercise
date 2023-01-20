using System.Collections;
using UnityEngine;

interface IAwaiter<T> // ���ʂ��󂯎�鑤�̂��߂̃C���^�[�t�F�C�X
{
    /// <summary>
    /// �������I���������ǂ����B
    /// </summary>
    bool IsCompleted { get; }

    /// <summary>
    /// �����̌��ʁB
    /// </summary>
    T Result { get; }
}

class Awaiter<T> : IAwaiter<T> // ���ʂ�ݒ肷�鑤�̎���
{
    public bool IsCompleted { get; private set; }

    public T Result { get; private set; }

    /// <summary>
    /// �������I�����Č��ʂ�ݒ肷��B
    /// </summary>
    public void SetResult(T result)
    {
        Result = result;
        IsCompleted = true;
    }
}

public class Sample : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(RunAsync());
    }

    private IEnumerator RunAsync()
    {
        while (true)
        {
            Debug.Log("�}�E�X�̃{�^�����͂�҂��܂�");
            yield return WaitForMouseButtonDown(out var awaiter);

            // Awaiter �̏I����́A�K�����ʂ��ۏ؂���Ă���
            Debug.Log($"�}�E�X��{awaiter.Result}�{�^����������܂���");
            yield return null;
        }
    }

    private IEnumerator WaitForMouseButtonDown(out IAwaiter<int> awaiter)
    {
        var awaiterImpl = new Awaiter<int>();
        var e = WaitForMouseButtonDown(awaiterImpl);
        awaiter = awaiterImpl;
        return e;
    }

    private IEnumerator WaitForMouseButtonDown(Awaiter<int> awaiter)
    {
        // �ǂ̃}�E�X�{�^���������ꂽ�̂��A���ʂ�Ԃ������B
        while (true)
        {
            for (var i = 0; i < 3; i++)
            {
                if (Input.GetMouseButtonDown(i))
                {
                    awaiter.SetResult(i); // �������I���E���ʂ�ݒ�
                    yield break;
                }
            }

            yield return null;
        }
    }
}