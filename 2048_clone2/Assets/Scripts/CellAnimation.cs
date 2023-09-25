using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CellAnimation : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI points;

    private float moveTime = .1f; // тайминг анимации движени€ - .1а - это 1/10 или (10% от 1)
    private float scaleTime = 1f; // тайминг анимации увеличени€

    private

     public void Move(Cell from, Cell to, bool isMerging) // €чейка из, €чейка в которую, объединениа ли
    {
        from.CancelAnimation(); // отмен€ем анимациию из которой мы движемс€
        to.SetAnimation(this); // передаем анимацию в которую движем€

        image.color = Field.Instance.Colors[from.Value]; // маскируемс€ под €чейку из которой идет перемешение
        points.text = from.Points.ToString();

        transform.position = from.transform.position; // поставить нашу позицию в такую же позицию, из которой мы едем
   
    
    }

    public void Destroy() // метод удалени€ анимации
    {

    }

}


// 2 01:26:30