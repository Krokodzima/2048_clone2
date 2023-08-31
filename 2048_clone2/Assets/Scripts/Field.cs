using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public static Field Instance; // ��������

    [Header("Field Properties")] // ��������� ������
    public float CellSize; // �������� �� ������ - ������ ������ 
    public float Spacing; // �������� �� ������ - ��������� ����� ��������
    public int FieldSize; // �������� �� ������ - ������ ���� (���������� ��������)

    [Space(5)]
    public Color[] Colors; // �������� ������ ��� ������ �����

    [Header("")]
    [Space(10)] // ������ �� ������ ����� ��������
    [SerializeField]
    private Cell cellPref; // ������ �� ������ ������
    [SerializeField]
    private RectTransform rt; // ��������� RectTransform �������

    private Cell[,] field;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }

    void Start()
    {
        CreateField(); // ����� ������ � ������
    }

    private void CreateField() // ����� �������� ����
    {
        field = new Cell[FieldSize, FieldSize];

        // ������ ������ � ������ ���� � ������ ���������(������)
        float fieldWidth = FieldSize * (CellSize + Spacing) + Spacing;
        rt.sizeDelta = new Vector2(fieldWidth, fieldWidth); // ������ �������

        // ������ ��������� x � y
        float startX = -(fieldWidth / 2) + (CellSize / 2) + Spacing;
        float startY = (fieldWidth / 2) - (CellSize / 2) - Spacing;

        for (int x = 0; x < FieldSize; x++) // ����� ������� ������ - ��������� ��� ����
        {

            for (int y = 0; y < FieldSize; y++)
            {
                var cell = Instantiate(cellPref, transform, false);
                var position = new Vector2(startX + (x * (CellSize + Spacing)), startY - (y * (CellSize + Spacing)));
                cell.transform.localPosition = position;   // ����� �� ��������� �������

                field[x, y] = cell;

                cell.SetValue(x, y, 0); // ������ �������� 0 ��� ����� 
            }

        }

    }
}
