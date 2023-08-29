using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public int X { get; private set; } // свойство X - индекс €чейки по горизонтали  в двумерном массиве
    public int Y { get; private set; }
    public int Value { get; private set; } // свойство Value(значение €чейки) может быть назначено внутри класса (private set), но снаружи только считано (get)
    public int Points => IsEmpty ? 0 : (int)Mathf.Pow(2, Value); // проверка, если €чейка пуста€, то 0, иначе (:) возводим 2ку в степень Value

    public bool IsEmpty => Value == 0; // €чейка пуста€, если Value = 0 (true), иначе есть очки (false)


    [SerializeField]
    private Image image; // задаем пункт в меню - цвет €чейки равный номиналу
    [SerializeField]
    private TextMeshProUGUI points;


    public void SetValue(int x, int y, int value) // метод дл€ задани€ x,y,value
    {
        X = x;
        Y = y;
        Value = value;
    }


}
