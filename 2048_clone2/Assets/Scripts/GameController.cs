using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance; // ��������

    public static int Points; // ����
    public static bool CanPlay; // ���� ����������� ���������� ����


    [SerializeField]
    private TextMeshProUGUI status;
    [SerializeField]
    private TextMeshProUGUI pointsText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame() // ����� ����
    {
        status.text = "";

        SetPoints(0);
        CanPlay = true;

        Field.Instance.PrepareField();
    }

    public void Win() // ������
    {
        CanPlay = false;
        status.text = "You Win!";
      //  Debug.Log("WIN!"); // ����� � ������� ��������� � ������
    }

    public void Loss() // ���������
    {
        CanPlay = false;
        status.text = "You Lose!";
        // Debug.Log("LOSS!"); // ����� � ������� ��������� � ������
    }

    public void AddPoints(int points)
    {
        SetPoints(Points + points);
    }

    private void SetPoints(int points)
    {
        Points = points;
        pointsText.text = Points.ToString();
    }
}


// 2 01:43:30