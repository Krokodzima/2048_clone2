using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance; // ��������

    public static bool CanPlay; // ���� ����������� ���������� ����

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

   private void Start()
    {
        StartGame();
    }

   public void StartGame() // ����� ����
    {
        CanPlay = true;

        Field.Instance.PrepareField();
    }

    public void Win() // ������
    {
        CanPlay = false;
        Debug.Log("WIN!");
    }

    public void Loss() // ���������
    {
        CanPlay = false;
        Debug.Log("LOSS!");
    }

}


// 2 00:40:15