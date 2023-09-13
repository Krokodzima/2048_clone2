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

    void Start()
    {

    }

   public void StartGame() // старт игры
    {
        
    }

    public void Win() // победа
    {
        Debug.Log("WIN!");
    }

    public void Loss() // поражение
    {
        Debug.Log("LOSS!");
    }

}


// 2 00:36:00