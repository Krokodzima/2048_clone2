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

    void Start()
    {

    }

   public void StartGame() // ����� ����
    {
        
    }

    public void Win() // ������
    {
        Debug.Log("WIN!");
    }

    public void Loss() // ���������
    {
        Debug.Log("LOSS!");
    }

}


// 2 00:36:00