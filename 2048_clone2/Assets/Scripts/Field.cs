using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [Header("Field Properties")] // заголовок панели
    public float CellSize; // свойство на панели - размер €чейки 
    public float Spacing; // свойство на панели - расто€ние между €чейками
    public int FieldSize; // свойство на панели - размер пол€ (измер€етс€ €чейками)

    [Header("")]
    [Space(10)] // отступ на панели между пунктами
    [SerializeField]
    private Cell cellPref; // ссылка на префаб €чейки
    [SerializeField]
    private RectTransform rt; // добавл€ет RectTransform объекту

    void Start()
    {
        CreateField(); // вызов метода в старте
    }

    private void CreateField() // метод создани€ пол€
    {
        // расчет ширины и высоты пол€
        float fieldWidth = FieldSize * (CellSize + Spacing) + Spacing;

        rt.sizeDelta = new Vector2(fieldWidth, fieldWidth); // дельта размера
    }
}
