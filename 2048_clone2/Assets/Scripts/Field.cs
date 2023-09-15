using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public static Field Instance; // синглтон

    [Header("Field Properties")] // заголовок панели
    public float CellSize; // свойство на панели - размер ячейки 
    public float Spacing; // свойство на панели - растояние между ячейками
    public int FieldSize; // свойство на панели - размер поля (измеряется ячейками)

    [Space(5)]
    public Color[] Colors; // добавить массив для выбора цвета

    [Header("")]
    [Space(10)] // отступ на панели между пунктами
    [SerializeField]
    private Cell cellPref; // ссылка на префаб ячейки
    [SerializeField]
    private RectTransform rt; // добавляет RectTransform объекту

    private Cell[,] field;

    private bool cellMoved; // двигалась ли ячейка

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }

    void Start()
    {
        CreateField(); // вызов метода в старте

        SwipeDetection.SwipeEvent += OnSwipeInput; // подписка на ивент
    }

    private void Update()
    {
        // #if и #endif - Директива препроцессора, код данного раздела будет выполнятся только в UNITY_EDITOR
        // данный код не будет скомпилирован на другие платформы, работает только в UNITY_EDITOR 
#if UNITY_EDITOR
        // привязка WASD к свайпу 
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

    private void OnSwipeInput(Vector2 direction) // подписка на ивент
    {
        if (!GameController.CanPlay)
            return;

        cellMoved = false; // сброс флага

        ResetCellFlags(); // сброс флага
        Move(direction);

        if (cellMoved)
        {
            GenerateRandomeCell();
            CheckGameResult();
        }
    }

    private void CreateField() // метод создания поля
    {
        field = new Cell[FieldSize, FieldSize];

        // расчет ширины и высоты поля с учетом спейсинга(бордер)
        float fieldWidth = FieldSize * (CellSize + Spacing) + Spacing;
        rt.sizeDelta = new Vector2(fieldWidth, fieldWidth); // дельта размера

        // расчет стартовые x и y
        float startX = -(fieldWidth / 2) + (CellSize / 2) + Spacing;
        float startY = (fieldWidth / 2) - (CellSize / 2) - Spacing;

        for (int x = 0; x < FieldSize; x++) // спавн префаба ячейки - заполнить все поле
        {

            for (int y = 0; y < FieldSize; y++)
            {
                var cell = Instantiate(cellPref, transform, false);
                var position = new Vector2(startX + (x * (CellSize + Spacing)), startY - (y * (CellSize + Spacing)));
                cell.transform.localPosition = position;   // спавн на стортовой позиции

                field[x, y] = cell;

                cell.SetValue(x, y, 0); // задать значение 0 для ячеек 
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

        for (int i = 0; i < 2; i++) // создать рандомную ячейку 2 раза
            GenerateRandomeCell();


        // тест
        // field[0, 0].SetValue(0, 0, 10);
        // field[0, 1].SetValue(0, 1, 10);
        // field[0, 2].SetValue(0, 2, 2);
        // field[0, 3].SetValue(0, 3, 2);
    }

    private void Move(Vector2 direction)
    {
        // определение точки с которой начнется "движение"
        int startXY = direction.x > 0 || direction.y < 0 ? FieldSize - 1 : 0; // стартовая ячейка равна, если движение вправо или(||) вниз, тогда (?) движение с самого края поля, иначе (:) 0 
        int dir = direction.x != 0 ? (int)direction.x : -(int)direction.y; // direction.x не равен (!=) 0, если так (?) то = direction.x (1 или -1), иначе (:) отрицательный direction.y (1 или -1)   

        for (int i = 0; i < FieldSize; i++)
        {
            for (int k = startXY; k >= 0 && k < FieldSize; k -= dir)
            {   // direction.x не равен 0, если так(?) то горизонатльный мувмент = k по y, иначе(:) вертикальный мувмент = y по k
                var cell = direction.x != 0 ? field[k, i] : field[i, k]; // находим ячейку текущую в которой мы находимся 

                if (cell.IsEmpty) // если ячейка пустая -пропускаем, идем дальше
                    continue;

                var cellToMerge = FindCellToMerge(cell, direction);  // проверка на возможность объединения
                if (cellToMerge != null)  // если возможность не нулевая, объединяем
                {
                    cell.MergeWithCell(cellToMerge);// merge
                    cellMoved = true;  // генерируется ячейка и проверка выигрыша/проигрыша
                    continue; // если нашли с кем объединиться - не нужно искать пустое пространство
                }

                var emptyCell = FindEmptyCell(cell, direction); // ищем пустую ячейку
                if (emptyCell != null) // если ячейка не пустая, то перемещаемся в эту ячейку
                {
                    cell.MoveToCell(emptyCell);//move
                    cellMoved = true;
                }
            }
        }

    }

    private Cell FindCellToMerge(Cell cell, Vector2 direction) // поиск ячейки с которой можно объединиться
    {
        // расчет стартовой ячейки
        int startX = cell.X + (int)direction.x; // startX = ячейка со значением + вектор направления
        int startY = cell.Y - (int)direction.y; // startY = ячейка со значением - вектор направления

        for (int x = startX, y = startY; x >= 0 && x < FieldSize && y >= 0 && y < FieldSize; x += (int)direction.x, y -= (int)direction.y)
        {
            if (field[x, y].IsEmpty) // если ячейка пустая, то идем дальше
                continue;
            // проверка, что в данной ячейке кол-во очков = кол-ву очков нашей "ячейки" и в данном ходу еще не меняла свое значение
            if (field[x, y].Value == cell.Value && !field[x, y].HasMerged)
                return field[x, y];

            break; // прекратить поиск
        }

        return null; // если нет клетки с которой можно объединиться
    }

    private Cell FindEmptyCell(Cell cell, Vector2 direction) // поиск пустой ячейки
    {
        Cell emptyCell = null;
        int startX = cell.X + (int)direction.x;
        int startY = cell.Y - (int)direction.y;

        for (int x = startX, y = startY; x >= 0 && x < FieldSize && y >= 0 && y < FieldSize; x += (int)direction.x, y -= (int)direction.y)
        {
            if (field[x, y].IsEmpty) // если текущая ячейка пустая, то emptyCell = эта ячейка
                emptyCell = field[x, y];

            else
                break;

        }

        return emptyCell;
    }

    private void CheckGameResult() // проверка выиигрыша/прогигрыша в игре
    {
        bool isLoss = true;

        for (var x = 0; x < FieldSize; x++)
        {
            for (var y = 0; y < FieldSize; y++)
            {
                if (field[x, y].Value == Cell.MaxValue) // если ячейка в массиве = константе MaxValue (11)
                {
                    GameController.Instance.Win();
                    return; // выйти
                }

                if (field[x, y].IsEmpty ||  // если есть одна пустая ячейка
                   FindCellToMerge(field[x, y], Vector2.left) || // есть ли слева ячейка с которой можно объединить
                   FindCellToMerge(field[x, y], Vector2.right) ||
                   FindCellToMerge(field[x, y], Vector2.up) ||
                   FindCellToMerge(field[x, y], Vector2.down))
                {
                    isLoss = false; // не проигрыш
                }
            }

        }

        if (isLoss) // проверка, если прогиграли, вызов метода "моражение"
            GameController.Instance.Loss();


    }


    private void ResetCellFlags() // сброс флагов массива ячеек циклом 
    {
        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                field[x, y].ResetFlags();
    }

    private void GenerateRandomeCell() // метод генерации рандомной ячейки
    {
        var emptyCells = new List<Cell>(); // создаем новый список типа <Cell> emptyCells

        for (int x = 0; x < FieldSize; x++) // проход по всем ячейкам
            for (int y = 0; y < FieldSize; y++)
                if (field[x, y].IsEmpty) // если ячейка пустая
                    emptyCells.Add(field[x, y]); // тогда добавить ее в список 

        int value = Random.Range(0, 10) == 0 ? 2 : 1; // если выпадает 0 то(?) 4ка(2^2), иначе (:) 2ка (2^1)

        if (emptyCells.Count == 0) // если кол-во пустых ячеек в списке emptyCells = 0
            throw new System.Exception("There in no any empty cell on the field"); // тогда выводим сообщение

        var cell = emptyCells[Random.Range(0, emptyCells.Count)]; // переменная cell - рандом от 0 до кол-ва пустых ячеек в списке emptyCells
        cell.SetValue(cell.X, cell.Y, value); // присвоить ячейке cell координаты и значение 
    }
}



/* private void GenerateRandomeCell() // метод генерации рандомной ячейки (не лучший вариант)
{
    // return; // включить для принудительного теста (запрет на генерацию новых ячеек)
    int x, y, itt = 0; // счетчик иттераций
                       // задание вероятности выпадения 4ки (1 к 10) и 2ки (9 к 10) 
    int value = Random.Range(0, 10) == 0 ? 2 : 1; // если выпадает 0 то(?) 4ка(2^2), иначе (:) 2ка (2^1)

    do
    {
        x = Random.Range(0, FieldSize);
        y = Random.Range(0, FieldSize);
    }
    while (!field[x, y].IsEmpty && itt++ < 200); // делать, пока есть не пустые ячейки и счетчик иттераций < 200

    if (itt == 200)
        throw new System.Exception("There in no any empty cell on the field");

    field[x, y].SetValue(x, y, value); // присвоить ячейке значение 
}
*/

// 2 00:55:07