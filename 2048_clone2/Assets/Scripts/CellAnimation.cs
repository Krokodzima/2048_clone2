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

    private float moveTime = .1f; // ������� �������� �������� - .1f - ��� 1/10 ��� (10% �� 1)
    private float scaleTime = .1f; // ������� �������� ����������

    private Sequence sequence; // ������������������ ��������

    public void Move(Cell from, Cell to, bool isMerging) // ������ ��, ������ � �������, ����������� ��
    {
        from.CancelAnimation(); // �������� ��������� �� ������� �� ��������
        to.SetAnimation(this); // �������� �������� � ������� �������

        image.color = Field.Instance.Colors[from.Value]; // ����������� ��� ������ �� ������� ���� �����������
        points.text = from.Points.ToString();
        points.color = from.Value <= 2 ? Field.Instance.LowValueColor : Field.Instance.HighValueColor; // ���� ������, ���� �������� ������ 2, �� ������ ����, ����� ������ ����


        transform.position = from.transform.position; // ��������� ���� ������� � ����� �� �������, �� ������� �� ����

        sequence = DOTween.Sequence(); // �������������� �����������

        // ������ �������� � ������� - ����������� �� ����� ������ � ������� ������
        sequence.Append(transform.DOMove(to.transform.position, moveTime).SetEase(Ease.InOutQuad)); // �������� �������� (����, �������, SetEase(������������))

        // ������ �������� ��������� �������
        if (isMerging)
        {
            sequence.AppendCallback(() =>
            {
                image.color = Field.Instance.Colors[to.Value]; // ����������� ��� ������ � ������� ���� �����������
                points.text = to.Points.ToString();
                points.color = to.Value <= 2 ? Field.Instance.LowValueColor : Field.Instance.HighValueColor; // ���� ������, ���� �������� ������ 2, �� ������ ����, ����� ������ ����

            });

            sequence.Append(transform.DOScale(1.2f, scaleTime)); // ��������� c 1 �� 1.2 �� scaleTime
            sequence.Append(transform.DOScale(1f, scaleTime)); // ����������� � �������� ���������
        }

        sequence.AppendCallback(() => // ��� ������� ���������� ����� ���� ��������
        {
            to.UpdateVisual(); // �������� ����� UpdateVisual �� Cell
            Destroy(gameObject); // ������� �������� �� �����

        });
    }

    public void Appear(Cell cell)
    {
        cell.CancelAnimation();
        cell.SetAnimation(this);

        image.color = Field.Instance.Colors[cell.Value]; // ����������� ��� ������ �� ������� ���� �����������
        points.text = cell.Points.ToString();
        points.color = cell.Value <= 2 ? Field.Instance.LowValueColor : Field.Instance.HighValueColor; // ���� ������, ���� �������� ������ 2, �� ������ ����, ����� ������ ����


        transform.position = cell.transform.position; // ��������� ���� ������� � ����� �� �������, �� ������� �� ����
        transform.localScale = Vector2.zero;

        sequence = DOTween.Sequence(); // �������������� �����������

        sequence.Append(transform.DOScale(1.2f, scaleTime * 2)); // ��������� c 1 �� 1.2 �� scaleTime
        sequence.Append(transform.DOScale(1f, scaleTime * 2)); // ����������� � �������� ���������
        sequence.AppendCallback(() => // ��� ������� ���������� ����� ���� ��������
        {
            cell.UpdateVisual(); // �������� ����� UpdateVisual �� Cell
            Destroy(gameObject); // ������� �������� �� �����

        });
    }

    public void Destroy() // ����� �������� ��������
    {
        sequence.Kill(); // ����� ������������������ ������, ��� ������� gameObject - ���������� ��������
        Destroy(gameObject); // ������� gameObject

    }

}


// 2 01:43:30