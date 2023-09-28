using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // плагин DOTween

public class CellAnimationController : MonoBehaviour
{
    public static CellAnimationController Instance; // синглтон

    [SerializeField]
    private CellAnimation animationPref;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        DOTween.Init(); // ВАЖНО!!! вызвать плагин, для инициализации переменных, чтобы не лагало
    }
    public void SmoothTransition(Cell from, Cell to, bool isMerging) // добавляет префаб на сцену и передает параметры
    {
        Instantiate(animationPref, transform, false).Move(from, to, isMerging); //Instantiate - создается объект animationPref и сразу обращается к Move
    }

    public void SmoothAppear(Cell cell)
    {
        Instantiate(animationPref, transform, false).Appear(cell); //Instantiate - создается объект animationPref и сразу обращается к Move

    }
}

// 2 01:43:30