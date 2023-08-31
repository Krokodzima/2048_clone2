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

    }
}
