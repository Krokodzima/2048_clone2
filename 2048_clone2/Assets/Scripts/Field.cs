using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [Header("Field Properties")] // ��������� ������
    public float CellSize; // �������� �� ������ - ������ ������ 
    public float Spacing; // �������� �� ������ - ��������� ����� ��������
    public int FieldSize; // �������� �� ������ - ������ ���� (���������� ��������)

    [Header("")]
    [Space(10)] // ������ �� ������ ����� ��������
    [SerializeField]
    private Cell cellPref; // ������ �� ������ ������
    [SerializeField]
    private RectTransform rt; // ��������� RectTransform �������

    void Start()
    {
        CreateField(); // ����� ������ � ������
    }

    private void CreateField() // ����� �������� ����
    {
        // ������ ������ � ������ ����
        float fieldWidth = FieldSize * (CellSize + Spacing) + Spacing;

        rt.sizeDelta = new Vector2(fieldWidth, fieldWidth); // ������ �������
    }
}
