using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance; // синглтон

    public static bool CanPlay; // флаг возможности продолжать игру

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
        CanPlay = true;

        Field.Instance.PrepareField();
    }

    public void Win() // победа
    {
        CanPlay = false;
        Debug.Log("WIN!");
    }

    public void Loss() // поражение
    {
        CanPlay = false;
        Debug.Log("LOSS!");
    }

}


// 2 00:40:15