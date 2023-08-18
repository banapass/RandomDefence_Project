using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using framework;

public class GameManager : Singleton<GameManager>
{
    private int currHeart;


    public bool IsBreakTime { get { return CurrentGameState == GameState.BreakTime; } }
    [field: SerializeField, ReadOnly] public GameState CurrentGameState { get; private set; }


    public static event Action<GameState> OnChangedGameState;
    public static event Action<int> OnLifeDamaged;

    private void OnEnable()
    {
        Monster.OnArrivalLastDestination += TakeDamage;
    }
    private void OnDisable()
    {
        Monster.OnArrivalLastDestination -= TakeDamage;
    }

    public void GameStart()
    {
        currHeart = Constants.MAX_LIFE;
    }

    public void ChangeGameState(GameState _gameState)
    {
        if (CurrentGameState == _gameState) return;

        CurrentGameState = _gameState;
        OnChangedGameState?.Invoke(_gameState);
    }

    public void TakeDamage()
    {
        currHeart--;

        if (currHeart <= 0)
        {
            ChangeGameState(GameState.GameOver);
            currHeart = 0;
        }

        OnLifeDamaged?.Invoke(currHeart);
    }

}
