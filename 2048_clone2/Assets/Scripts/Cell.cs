using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public int X { get; private set; } // свойство X - индекс €чейки по горизонтали  в двумерном массиве
    public int Y { get; private set; }
    public int Value { get; private set; } // свойство Value(значение €чейки) может быть назначено внутри класса (private set), но снаружи только считано (get)
    public int Points => IsEmpty ? 0 : (int)Mathf.Pow(2, Value); // проверка, если €чейка пуста€, то 0, иначе (:) возводим 2ку в степень Value
    public bool HasMerged { get; private set; }

    public bool IsEmpty => Value == 0; // €чейка пуста€, если Value = 0 (true), иначе есть очки (false)

    public const int MaxValue = 11;

    [SerializeField]
    private Image image; // задаем пункт в меню - цвет €чейки равный номиналу
    [SerializeField]
    private TextMeshProUGUI points;

    private CellAnimation currentAnimation; // переменна€, к-€ хранить текущую анимацию


    public void SetValue(int x, int y, int value, bool updateUI = true) // метод дл€ задани€ x,y,value 
    {
        X = x;
        Y = y;
        Value = value;

        if (updateUI) // усли updateUI = true, то вызываем метод UpdateVisual()
            UpdateVisual();
    }

    public void IncreaseValue()
    {
        Value++;
        HasMerged = true;

       // UpdateVisual(); // без апдейта - подлый баг (искал 2 дн€)

        GameController.Instance.AddPoints(Points);
    }

    public void ResetFlags()
    {
        HasMerged = false;
    }

    public void MergeWithCell(Cell otherCell)
    {
        CellAnimationController.Instance.SmoothAnimation(this, otherCell, true); // вызов анимации (откуда, куда, объедин€емс€ ли)

        otherCell.IncreaseValue();
        SetValue(X, Y, 0);
    }

    public void MoveToCell(Cell target)
    {
        CellAnimationController.Instance.SmoothAnimation(this, target, false); // вызов анимации (откуда, куда, объедин€емс€ ли)


        target.SetValue(target.X, target.Y, Value, false);
        SetValue(X, Y, 0);
    }

    public void UpdateVisual()// метод отображени€ количества очков и цвет €чейки
    {
        points.text = IsEmpty ? string.Empty : Points.ToString(); // проверка, если поле points пустое, то строка пуста€, иначе передать значение Points
        points.color = Value <= 2 ? Field.Instance.LowValueColor : Field.Instance.HighValueColor; // цвет текста, если значение меньше 2, то первый цвет, иначе второй цвет

        image.color = Field.Instance.Colors[Value];// присвоить цвет image.component   
    }

    public void SetAnimation(CellAnimation animation) // 
    {
        currentAnimation = animation;
    }

    public void CancelAnimation() // если currentAnimation не пуста€, то вызвать метод  Destroy
    {
        if (currentAnimation != null)
            currentAnimation.Destroy();
    }

}

// 2 01:43:30