using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CellAnimation : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI points;

    private float moveTime = .1f; // ������� �������� �������� - .1� - ��� 1/10 ��� (10% �� 1)
    private float scaleTime = 1f; // ������� �������� ����������

    private

     public void Move(Cell from, Cell to, bool isMerging) // ������ ��, ������ � �������, ����������� ��
    {
        from.CancelAnimation(); // �������� ��������� �� ������� �� ��������
        to.SetAnimation(this); // �������� �������� � ������� �������

        image.color = Field.Instance.Colors[from.Value]; // ����������� ��� ������ �� ������� ���� �����������
        points.text = from.Points.ToString();

        transform.position = from.transform.position; // ��������� ���� ������� � ����� �� �������, �� ������� �� ����
   
    
    }

    public void Destroy() // ����� �������� ��������
    {

    }

}


// 2 01:26:30