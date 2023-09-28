using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    public static event OnSwipeInput SwipeEvent;
    public delegate void OnSwipeInput(Vector2 direction); // делегат для ивента (объявление сигнатуры метода, к.й. подпишется на ивент)

    private Vector2 tapPosition;
    private Vector2 swipeDelta;

    private float deadZone = 80; // минимальная длина от которой засчитается свайп

    private bool isSwiping; // для свайпа мышкой
    private bool isMobile; // для мобильной платформы


    private void Start()
    {
        isMobile = Application.isMobilePlatform;
    }

    private void Update()
    {
        if (!isMobile) // если не мобильная платформа
        {
            if (Input.GetMouseButtonDown(0)) // если нажата кнопка мыши
            {
                isSwiping = true; // режим свайпа мышкой - true
                tapPosition = Input.mousePosition; // где тап мышкой - оттуда свайп
            }
            else if (Input.GetMouseButtonUp(0)) // где мышь отпустили, там закочить свайп
                ResetSwipe();
        }
        else
        {
            if (Input.touchCount > 0) // если мобилка - проверить нажание
            {
                if (Input.touches[0].phase == TouchPhase.Began) // получение доступа к нажатию через массив
                {
                    isSwiping = true;
                    tapPosition = Input.touches[0].position;
                }
                else if (Input.touches[0].phase == TouchPhase.Canceled || Input.touches[0].phase == TouchPhase.Ended)
                    ResetSwipe();
            }
        }

        CheckSwipe();
    }

    private void CheckSwipe() // метод определения свайпа
    {
        swipeDelta = Vector2.zero; // обнулили swipeDelta
        if (isSwiping)
        {
            if (!isMobile && Input.GetMouseButton(0)) // если не мобилка и ЛКМ зажата
                swipeDelta = (Vector2)Input.mousePosition - tapPosition; // 
            else if (Input.touchCount > 0) // если на телефоне больше одного нажатия
                swipeDelta = Input.touches[0].position - tapPosition; // самый первый тач [0] - tapPosition
        }

        if (swipeDelta.magnitude > deadZone)  // проверка, если вектор свайпа больше  deadZone
        {
            if (SwipeEvent != null)  // вызов ивента (обязательно проверить, что ивент не равен (!=) null)
            {
                // опредедить направление свайпа (вектор swipeDelta имеет длину и направление, определить, что больше X или Y
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y)) // если модуль wipeDelta.x больше модуля wipeDelta.y
                    SwipeEvent(swipeDelta.x > 0 ? Vector2.right : Vector2.left); // swipeDelta.x больше 0, тогда вправо, иначе влево
                else
                    SwipeEvent(swipeDelta.y > 0 ? Vector2.up : Vector2.down); // swipeDelta.y больше 0, тогда вверх[, иначе вниз
            }

            ResetSwipe();
        }

    }

    private void ResetSwipe() // метод сброса свайпа
    {
        isSwiping = false;

        tapPosition = Vector2.zero;
        swipeDelta = Vector2.zero;
    }


}

// 2 01:43:30