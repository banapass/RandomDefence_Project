using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using framework;

public class GameManager : Singleton<GameManager>
{
    public bool IsBreakTime { get { return CurrentGameState == GameState.BreakTime; } }
    [field: SerializeField, ReadOnly] public GameState CurrentGameState { get; private set; }
    public static event Action<GameState> OnChangedGameState;

    public void ChangeGameState(GameState _gameState)
    {
        if (CurrentGameState == _gameState) return;

        CurrentGameState = _gameState;
        OnChangedGameState?.Invoke(_gameState);
    }
}
