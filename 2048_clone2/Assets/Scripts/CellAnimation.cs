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

    private float moveTime = .1f; // тайминг анимации движени€ - .1f - это 1/10 или (10% от 1)
    private float scaleTime = .1f; // тайминг анимации увеличени€

    private Sequence sequence; // последовательность анимаций

    public void Move(Cell from, Cell to, bool isMerging) // €чейка из, €чейка в которую, объединениа ли
    {
        from.CancelAnimation(); // отмен€ем анимациию из которой мы движемс€
        to.SetAnimation(this); // передаем анимацию в которую движем€

        image.color = Field.Instance.Colors[from.Value]; // маскируемс€ под €чейку из которой идет перемешение
        points.text = from.Points.ToString();
        points.color = from.Value <= 2 ? Field.Instance.LowValueColor : Field.Instance.HighValueColor; // цвет текста, если значение меньше 2, то первый цвет, иначе второй цвет


        transform.position = from.transform.position; // поставить нашу позицию в такую же позицию, из которой мы едем

        sequence = DOTween.Sequence(); // инициализируем очередность

        // перва€ анимаци€ в очереди - пермедщение из нашей €чейки в целевую €чейку
        sequence.Append(transform.DOMove(to.transform.position, moveTime).SetEase(Ease.InOutQuad)); // добавить анимацию (куда, тайминг, SetEase(нелинейность))

        // втора€ анимаци€ изменени€ размера
        if (isMerging)
        {
            sequence.AppendCallback(() =>
            {
                image.color = Field.Instance.Colors[to.Value]; // маскируемс€ под €чейку в которую идет перемешение
                points.text = to.Points.ToString();
                points.color = to.Value <= 2 ? Field.Instance.LowValueColor : Field.Instance.HighValueColor; // цвет текста, если значение меньше 2, то первый цвет, иначе второй цвет

            });

            sequence.Append(transform.DOScale(1.2f, scaleTime)); // увеличить c 1 до 1.2 за scaleTime
            sequence.Append(transform.DOScale(1f, scaleTime)); // возвращение к исходным значени€м
        }

        sequence.AppendCallback(() => // код который выполнитс€ после всех анимаций
        {
            to.UpdateVisual(); // вызывает метод UpdateVisual из Cell
            Destroy(gameObject); // удалить анимацию со сцены

        });
    }

    public void Appear(Cell cell)
    {
        cell.CancelAnimation();
        cell.SetAnimation(this);

        image.color = Field.Instance.Colors[cell.Value]; // маскируемс€ под €чейку из которой идет перемешение
        points.text = cell.Points.ToString();
        points.color = cell.Value <= 2 ? Field.Instance.LowValueColor : Field.Instance.HighValueColor; // цвет текста, если значение меньше 2, то первый цвет, иначе второй цвет


        transform.position = cell.transform.position; // поставить нашу позицию в такую же позицию, из которой мы едем
        transform.localScale = Vector2.zero;

        sequence = DOTween.Sequence(); // инициализируем очередность

        sequence.Append(transform.DOScale(1.2f, scaleTime * 2)); // увеличить c 1 до 1.2 за scaleTime
        sequence.Append(transform.DOScale(1f, scaleTime * 2)); // возвращение к исходным значени€м
        sequence.AppendCallback(() => // код который выполнитс€ после всех анимаций
        {
            cell.UpdateVisual(); // вызывает метод UpdateVisual из Cell
            Destroy(gameObject); // удалить анимацию со сцены

        });
    }

    public void Destroy() // метод удалени€ анимации
    {
        sequence.Kill(); // убить последовательность прежде, чем удалить gameObject - остановить аницацию
        Destroy(gameObject); // удалить gameObject

    }

}


// 2 01:43:30