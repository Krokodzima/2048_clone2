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

    private bool cellMoved; // ��������� �� ������

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }

    void Start()
    {
        CreateField(); // ����� ������ � ������

        SwipeDetection.SwipeEvent += OnWsipeInput; // �������� �� �����
    }

    private void OnWsipeInput(Vector2 direction) // �������� �� �����
    {
        cellMoved = false; // ����� �����

        ResetCellFlags(); // ����� �����
        Move(direction);

        if (cellMoved)
        {
            GenerateRandomeCell();
            CheckGameResult();
        }
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

        for (int i = 0; i < 2; i++) // ������� ��������� ������ 2 ����
            GenerateRandomeCell();
    }



    private void Move(Vector2 direction)
    {
        // ����������� ����� � ������� �������� "��������"
        int startXY = direction.x > 0 || direction.y < 0 ? FieldSize - 1 : 0; // ��������� ������ �����, ���� �������� ������ ���(||) ����, ����� (?) �������� � ������ ���� ����, ����� (:) 0 
        int dir = direction.x != 0 ? (int)direction.x : -(int)direction.y; // direction.x �� ����� (!=) 0, ���� ��� (?) �� = direction.x (1 ��� -1), ����� (:) ������������� direction.y (1 ��� -1)   

        for (int i = 0; i < FieldSize; i++)
        {
            for (int k = startXY; k >= 0 && k < FieldSize; k -= dir)
            {   // direction.x �� ����� 0, ���� ���(?) �� �������������� ������� = k �� y, �����(:) ������������ ������� = y �� k
                var cell = direction.x != 0 ? field[k, i] : field[i, k]; // ������� ������ ������� � ������� �� ��������� 

                if (cell.IsEmpty) // ���� ������ ������ -����������, ���� ������
                    continue;

                var cellToMerge = FindCellToMerge(cell, direction);  // �������� �� ����������� �����������
                if (cellToMerge != null)  // ���� ����������� �� �������, ����������
                {
                  cell.MergeWithCell(cellToMerge);// merge
                    cellMoved = true;  // ������������ ������ � �������� ��������/���������
                    continue; // ���� ����� � ��� ������������ - �� ����� ������ ������ ������������
                }

                var emptyCell = FindEmptyCell(cell, direction); // ���� ������ ������
                if (emptyCell != null) // ���� ������ �� ������, �� ������������ � ��� ������
                {
                    cell.MoveToCell(emptyCell);//move
                    cellMoved = true;
                }
            }
        }

    }

    private Cell FindCellToMerge(Cell cell, Vector2 direction) // ����� ������ � ������� ����� ������������
    {
        // ������ ��������� ������
        int startX = cell.X + (int)direction.x; // startX = ������ �� ��������� + ������ �����������
        int startY = cell.Y - (int)direction.y; // startY = ������ �� ��������� - ������ �����������

        for (int x = startX, y = startY; x >= 0 && x < FieldSize && y >= 0 && y < FieldSize; x += (int)direction.x, y -= (int)direction.y)
        {
            if (field[x, y].IsEmpty) // ���� ������ ������, �� ���� ������
                continue;
            // ��������, ��� � ������ ������ ���-�� ����� = ���-�� ����� ����� "������" � � ������ ���� ��� �� ������ ���� ��������
            if (field[x, y].Value == cell.Value && !field[x, y].HasMerged)
                return field[x, y];

            break; // ���������� �����
        }

        return null; // ���� ��� ������ � ������� ����� ������������
    }

    private Cell FindEmptyCell(Cell cell, Vector2 direction) // ����� ������ ������
    {
        Cell emptyCell = null;
        int startX = cell.X + (int)direction.x;
        int startY = cell.Y - (int)direction.y;

        for (int x = startX, y = startY; x >= 0 && x < FieldSize && y >= 0 && y < FieldSize; x += (int)direction.x, y -= (int)direction.y)
        {
            if (field[x, y].IsEmpty) // ���� ������� ������ ������, �� emptyCell = ��� ������
                emptyCell = field[x, y];

            else
                break;

        }



        return emptyCell;
    }

    private void CheckGameResult() // �������� ���������/���������� � ����
    {

    }


    private void ResetCellFlags() // ����� ������ ������� ����� ������ 
    {
        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                field[x, y].ResetFlags();
    }

    private void GenerateRandomeCell() // ����� ��������� ��������� ������
    {
        int x, y, itt = 0; // ������� ���������
        // ������� ����������� ��������� 4�� (1 � 10) � 2�� (9 � 10) 
        int value = Random.Range(0, 10) == 0 ? 2 : 1; // ���� �������� 0 ��(?) 4��(2^2), ����� (:) 2�� (2^1)

        do
        {
            x = Random.Range(0, FieldSize);
            y = Random.Range(0, FieldSize);
        }
        while (!field[x, y].IsEmpty && itt++ < 200); // ������, ���� ���� �� ������ ������ � ������� ��������� < 200
        if (itt == 200)
            throw new System.Exception("There in no any empty cell on the field");

        field[x, y].SetValue(x, y, value); // ��������� ������ �������� 
    }
}

// 1 02:50:37