using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance; // синглтон

    public static int Points; // очки
    public static bool CanPlay; // флаг возможности продолжать игру


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

    public void StartGame() // старт игры
    {
        status.text = "";

        SetPoints(0);
        CanPlay = true;

        Field.Instance.PrepareField();
    }

    public void Win() // победа
    {
        CanPlay = false;
        status.text = "You Win!";
      //  Debug.Log("WIN!"); // вывод в консоль сообщения о победе
    }

    public void Loss() // поражение
    {
        CanPlay = false;
        status.text = "You Lose!";
        // Debug.Log("LOSS!"); // вывод в консоль сообщения о победе
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