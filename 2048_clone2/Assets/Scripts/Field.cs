using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public static Field Instance; // синглтон

    [Header("Field Properties")] // заголовок панели
    public float CellSize; // свойство на панели - размер €чейки 
    public float Spacing; // свойство на панели - расто€ние между €чейками
    public int FieldSize; // свойство на панели - размер пол€ (измер€етс€ €чейками)

    [Space(5)]
    public Color[] Colors; // добавить массив дл€ выбора цвета

    [Header("")]
    [Space(10)] // отступ на панели между пунктами
    [SerializeField]
    private Cell cellPref; // ссылка на префаб €чейки
    [SerializeField]
    private RectTransform rt; // добавл€ет RectTransform объекту

    private Cell[,] field;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }

    void Start()
    {
        CreateField(); // вызов метода в старте
    }

    private void CreateField() // метод создани€ пол€
    {
        field = new Cell[FieldSize, FieldSize];

        // расчет ширины и высоты пол€ с учетом спейсинга(бордер)
        float fieldWidth = FieldSize * (CellSize + Spacing) + Spacing;
        rt.sizeDelta = new Vector2(fieldWidth, fieldWidth); // дельта размера

        // расчет стартовые x и y
        float startX = -(fieldWidth / 2) + (CellSize / 2) + Spacing;
        float startY = (fieldWidth / 2) - (CellSize / 2) - Spacing;

        for (int x = 0; x < FieldSize; x++) // спавн префаба €чейки - заполнить все поле
        {

            for (int y = 0; y < FieldSize; y++)
            {
                var cell = Instantiate(cellPref, transform, false);
                var position = new Vector2(startX + (x * (CellSize + Spacing)), startY - (y * (CellSize + Spacing)));
                cell.transform.localPosition = position;   // спавн на стортовой позиции

                field[x, y] = cell;

                cell.SetValue(x, y, 0); // задать значение 0 дл€ €чеек 
            }

        }

        for (int i = 0; i < 2; i++) // создать рандомную €чейку 2 раза
            GenerateRandomeCell();
    }

    private void GenerateRandomeCell() // метод генерации рандомной €чейки
    {
        int x, y, itt =0; // счетчик иттераций
        // задание веро€тности выпадени€ 4ки (1 к 10) и 2ки (9 к 10) 
        int value = Random.Range(0, 10) == 0 ? 2 : 1; // если выпадает 0 то(?) 4ка(2^2), иначе (:) 2ка (2^1)

        do
        {
            x = Random.Range(0, FieldSize);
            y = Random.Range(0, FieldSize);
        }
        while (!field[x,y].IsEmpty && itt++ < 200); // делать, пока есть не пустые €чейки и счетчик иттераций < 200
        if (itt == 200) 
            throw new System.Exception("There in no any empty cell on the field");

        field[x, y].SetValue(x, y, value); // присвоить €чейке значение 
    }
}
