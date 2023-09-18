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

        SwipeDetection.SwipeEvent += OnSwipeInput; // �������� �� �����
    }

    private void Update()
    {
        // #if � #endif - ��������� �������������, ��� ������� ������� ����� ���������� ������ � UNITY_EDITOR
        // ������ ��� �� ����� ������������� �� ������ ���������, �������� ������ � UNITY_EDITOR 
#if UNITY_EDITOR
        // �������� WASD � ������ 
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            OnSwipeInput(Vector2.left);
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            OnSwipeInput(Vector2.right);
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            OnSwipeInput(Vector2.up);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            OnSwipeInput(Vector2.down);

#endif
    }

    private void OnSwipeInput(Vector2 direction) // �������� �� �����
    {
        if (!GameController.CanPlay)
            return;

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
    }


    public void PrepareField()
    {
        if (field == null)
            CreateField();

            for (int x = 0; x < FieldSize; x++)
                for (int y = 0; y < FieldSize; y++)
                    field[x, y].SetValue(x, y, 0);

        for (int i = 0; i < 2; i++) // ������� ��������� ������ 2 ����
            GenerateRandomeCell();


        // ����
        // field[0, 0].SetValue(0, 0, 10);
        // field[0, 1].SetValue(0, 1, 10);
        // field[0, 2].SetValue(0, 2, 2);
        // field[0, 3].SetValue(0, 3, 2);
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
        bool isLoss = true;

        for (var x = 0; x < FieldSize; x++)
        {
            for (var y = 0; y < FieldSize; y++)
            {
                if (field[x, y].Value == Cell.MaxValue) // ���� ������ � ������� = ��������� MaxValue (11)
                {
                    GameController.Instance.Win();
                    return; // �����
                }

                if (field[x, y].IsEmpty ||  // ���� ���� ���� ������ ������
                   FindCellToMerge(field[x, y], Vector2.left) || // ���� �� ����� ������ � ������� ����� ����������
                   FindCellToMerge(field[x, y], Vector2.right) ||
                   FindCellToMerge(field[x, y], Vector2.up) ||
                   FindCellToMerge(field[x, y], Vector2.down))
                {
                    isLoss = false; // �� ��������
                }
            }

        }

        if (isLoss) // ��������, ���� ����������, ����� ������ "���������"
            GameController.Instance.Loss();


    }


    private void ResetCellFlags() // ����� ������ ������� ����� ������ 
    {
        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                field[x, y].ResetFlags();
    }

    private void GenerateRandomeCell() // ����� ��������� ��������� ������
    {
        var emptyCells = new List<Cell>(); // ������� ����� ������ ���� <Cell> emptyCells

        for (int x = 0; x < FieldSize; x++) // ������ �� ���� �������
            for (int y = 0; y < FieldSize; y++)
                if (field[x, y].IsEmpty) // ���� ������ ������
                    emptyCells.Add(field[x, y]); // ����� �������� �� � ������ 

        int value = Random.Range(0, 10) == 0 ? 2 : 1; // ���� �������� 0 ��(?) 4��(2^2), ����� (:) 2�� (2^1)

        if (emptyCells.Count == 0) // ���� ���-�� ������ ����� � ������ emptyCells = 0
            throw new System.Exception("There in no any empty cell on the field"); // ����� ������� ���������

        var cell = emptyCells[Random.Range(0, emptyCells.Count)]; // ���������� cell - ������ �� 0 �� ���-�� ������ ����� � ������ emptyCells
        cell.SetValue(cell.X, cell.Y, value); // ��������� ������ cell ���������� � �������� 
    }
}



/* private void GenerateRandomeCell() // ����� ��������� ��������� ������ (�� ������ �������)
{
    // return; // �������� ��� ��������������� ����� (������ �� ��������� ����� �����)
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
*/

// 2 00:55:07