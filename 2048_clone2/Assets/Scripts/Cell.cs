using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public int X { get; private set; } // �������� X - ������ ������ �� �����������  � ��������� �������
    public int Y { get; private set; }
    public int Value { get; private set; } // �������� Value(�������� ������) ����� ���� ��������� ������ ������ (private set), �� ������� ������ ������� (get)
    public int Points => IsEmpty ? 0 : (int)Mathf.Pow(2, Value); // ��������, ���� ������ ������, �� 0, ����� (:) �������� 2�� � ������� Value

    public bool IsEmpty => Value == 0; // ������ ������, ���� Value = 0 (true), ����� ���� ���� (false)


    [SerializeField]
    private Image image; // ������ ����� � ���� - ���� ������ ������ ��������
    [SerializeField]
    private TextMeshProUGUI points;


    public void SetValue(int x, int y, int value) // ����� ��� ������� x,y,value
    {
        X = x;
        Y = y;
        Value = value;
    }


}
