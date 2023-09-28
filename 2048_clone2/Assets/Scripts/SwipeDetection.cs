using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    public static event OnSwipeInput SwipeEvent;
    public delegate void OnSwipeInput(Vector2 direction); // ������� ��� ������ (���������� ��������� ������, �.�. ���������� �� �����)

    private Vector2 tapPosition;
    private Vector2 swipeDelta;

    private float deadZone = 80; // ����������� ����� �� ������� ����������� �����

    private bool isSwiping; // ��� ������ ������
    private bool isMobile; // ��� ��������� ���������


    private void Start()
    {
        isMobile = Application.isMobilePlatform;
    }

    private void Update()
    {
        if (!isMobile) // ���� �� ��������� ���������
        {
            if (Input.GetMouseButtonDown(0)) // ���� ������ ������ ����
            {
                isSwiping = true; // ����� ������ ������ - true
                tapPosition = Input.mousePosition; // ��� ��� ������ - ������ �����
            }
            else if (Input.GetMouseButtonUp(0)) // ��� ���� ���������, ��� �������� �����
                ResetSwipe();
        }
        else
        {
            if (Input.touchCount > 0) // ���� ������� - ��������� �������
            {
                if (Input.touches[0].phase == TouchPhase.Began) // ��������� ������� � ������� ����� ������
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

    private void CheckSwipe() // ����� ����������� ������
    {
        swipeDelta = Vector2.zero; // �������� swipeDelta
        if (isSwiping)
        {
            if (!isMobile && Input.GetMouseButton(0)) // ���� �� ������� � ��� ������
                swipeDelta = (Vector2)Input.mousePosition - tapPosition; // 
            else if (Input.touchCount > 0) // ���� �� �������� ������ ������ �������
                swipeDelta = Input.touches[0].position - tapPosition; // ����� ������ ��� [0] - tapPosition
        }

        if (swipeDelta.magnitude > deadZone)  // ��������, ���� ������ ������ ������  deadZone
        {
            if (SwipeEvent != null)  // ����� ������ (����������� ���������, ��� ����� �� ����� (!=) null)
            {
                // ���������� ����������� ������ (������ swipeDelta ����� ����� � �����������, ����������, ��� ������ X ��� Y
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y)) // ���� ������ wipeDelta.x ������ ������ wipeDelta.y
                    SwipeEvent(swipeDelta.x > 0 ? Vector2.right : Vector2.left); // swipeDelta.x ������ 0, ����� ������, ����� �����
                else
                    SwipeEvent(swipeDelta.y > 0 ? Vector2.up : Vector2.down); // swipeDelta.y ������ 0, ����� �����[, ����� ����
            }

            ResetSwipe();
        }

    }

    private void ResetSwipe() // ����� ������ ������
    {
        isSwiping = false;

        tapPosition = Vector2.zero;
        swipeDelta = Vector2.zero;
    }


}

// 2 01:43:30