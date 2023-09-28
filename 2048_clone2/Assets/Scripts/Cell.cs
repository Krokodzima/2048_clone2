using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public int X { get; private set; } // �������� X - ������ ������ �� �����������  � ��������� �������
    public int Y { get; private set; }
    public int Value { get; private set; } // �������� Value(�������� ������) ����� ���� ��������� ������ ������ (private set), �� ������� ������ ������� (get)
    public int Points => IsEmpty ? 0 : (int)Mathf.Pow(2, Value); // ��������, ���� ������ ������, �� 0, ����� (:) �������� 2�� � ������� Value
    public bool HasMerged { get; private set; }

    public bool IsEmpty => Value == 0; // ������ ������, ���� Value = 0 (true), ����� ���� ���� (false)

    public const int MaxValue = 11;

    [SerializeField]
    private Image image; // ������ ����� � ���� - ���� ������ ������ ��������
    [SerializeField]
    private TextMeshProUGUI points;

    private CellAnimation currentAnimation; // ����������, �-� ������� ������� ��������


    public void SetValue(int x, int y, int value, bool updateUI = true) // ����� ��� ������� x,y,value 
    {
        X = x;
        Y = y;
        Value = value;

        if (updateUI) // ���� updateUI = true, �� �������� ����� UpdateVisual()
            UpdateVisual();
    }

    public void IncreaseValue()
    {
        Value++;
        HasMerged = true;

       // UpdateVisual(); // ��� ������� - ������ ��� (����� 2 ���)

        GameController.Instance.AddPoints(Points);
    }

    public void ResetFlags()
    {
        HasMerged = false;
    }

    public void MergeWithCell(Cell otherCell)
    {
        CellAnimationController.Instance.SmoothAnimation(this, otherCell, true); // ����� �������� (������, ����, ������������ ��)

        otherCell.IncreaseValue();
        SetValue(X, Y, 0);
    }

    public void MoveToCell(Cell target)
    {
        CellAnimationController.Instance.SmoothAnimation(this, target, false); // ����� �������� (������, ����, ������������ ��)


        target.SetValue(target.X, target.Y, Value, false);
        SetValue(X, Y, 0);
    }

    public void UpdateVisual()// ����� ����������� ���������� ����� � ���� ������
    {
        points.text = IsEmpty ? string.Empty : Points.ToString(); // ��������, ���� ���� points ������, �� ������ ������, ����� �������� �������� Points
        points.color = Value <= 2 ? Field.Instance.LowValueColor : Field.Instance.HighValueColor; // ���� ������, ���� �������� ������ 2, �� ������ ����, ����� ������ ����

        image.color = Field.Instance.Colors[Value];// ��������� ���� image.component   
    }

    public void SetAnimation(CellAnimation animation) // 
    {
        currentAnimation = animation;
    }

    public void CancelAnimation() // ���� currentAnimation �� ������, �� ������� �����  Destroy
    {
        if (currentAnimation != null)
            currentAnimation.Destroy();
    }

}

// 2 01:43:30