using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // ������ DOTween

public class CellAnimationController : MonoBehaviour
{
    public static CellAnimationController Instance; // ��������

    [SerializeField]
    private CellAnimation animationPref;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        DOTween.Init(); // �����!!! ������� ������, ��� ������������� ����������, ����� �� ������
    }
    public void SmoothTransition(Cell from, Cell to, bool isMerging) // ��������� ������ �� ����� � �������� ���������
    {
        Instantiate(animationPref, transform, false).Move(from, to, isMerging); //Instantiate - ��������� ������ animationPref � ����� ���������� � Move
    }

    public void SmoothAppear(Cell cell)
    {
        Instantiate(animationPref, transform, false).Appear(cell); //Instantiate - ��������� ������ animationPref � ����� ���������� � Move

    }
}

// 2 01:43:30