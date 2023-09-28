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

    private float moveTime = .1f; // тайминг анимации движения - .1f - это 1/10 или (10% от 1)
    private float scaleTime = 1f; // тайминг анимации увеличения

    private Sequence sequence; // последовательность анимаций

     public void Move(Cell from, Cell to, bool isMerging) // ячейка из, ячейка в которую, объединениа ли
    {
        from.CancelAnimation(); // отменяем анимациию из которой мы движемся
        to.SetAnimation(this); // передаем анимацию в которую движемя

        image.color = Field.Instance.Colors[from.Value]; // маскируемся под ячейку из которой идет перемешение
        points.text = from.Points.ToString();

        transform.position = from.transform.position; // поставить нашу позицию в такую же позицию, из которой мы едем

        sequence = DOTween.Sequence(); // инициализируем очередность
         
        // первая анимация в очереди - пермедщение из нашей ячейки в целевую ячейку
        sequence.Append(transform.DOMove(to.transform.position, moveTime).SetEase(Ease.InOutQuad)); // добавить анимацию (куда, тайминг, SetEase(нелинейность))

        // вторая анимация изменения размера
        if (isMerging)
        {
            sequence.AppendCallback(() =>
            {
                image.color = Field.Instance.Colors[to.Value]; // маскируемся под ячейку в которую идет перемешение
                points.text = to.Points.ToString();
            });

            sequence.Append(transform.DOScale(1.2f, scaleTime)); // увеличить c 1 до 1.2 за scaleTime
            sequence.Append(transform.DOScale(1f, scaleTime)); // возвращение к исходным значениям
        }

        sequence.AppendCallback(() => // код который выполнится после всех анимаций
        {
            to.UpdateVisual(); // вызывает метод UpdateVisual из Cell
            Destroy(gameObject); // удалить анимацию со сцены

        });


    }

    public void Destroy() // метод удаления анимации
    {
        sequence.Kill(); // убить последовательность прежде, чем удалить gameObject - остановить аницацию
        Destroy(gameObject); // удалить gameObject

    }

}


// 2 01:43:30