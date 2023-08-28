using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using framework;

public class GameManager : Singleton<GameManager>
{
    private int currHeart;
    private int currGold;


    public bool IsBreakTime { get { return CurrentGameState == GameState.BreakTime; } }
    [field: SerializeField, ReadOnly] public GameState CurrentGameState { get; private set; }


    public static event Action<GameState> OnChangedGameState;
    public static event Action<int> OnLifeDamaged;
    public static event Action<int> OnChangedGold;

    private void OnEnable()
    {
        Monster.OnArrivalLastDestination += TakeDamage;
        Monster.OnDeath += OnMonsterDeath;
    }
    private void OnDisable()
    {
        Monster.OnArrivalLastDestination -= TakeDamage;
        Monster.OnDeath -= OnMonsterDeath;
    }

    public void GameStart()
    {
        currHeart = Constants.MAX_LIFE;
        GainGold(Constants.START_GOLD);
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

    public bool UseGold(int _gold)
    {
        if (currGold >= _gold)
        {
            currGold -= _gold;
            OnChangedGold?.Invoke(currGold);
            return true;
        }
        else
        {
            return false;
        }
    }
    public void GainGold(int _cost)
    {
        currGold += _cost;
        OnChangedGold?.Invoke(currGold);
    }

    public bool IsCanBuy(int _useGold)
    {
        return currGold >= _useGold;
    }
    public void OnMonsterDeath(Monster _monster)
    {
        GainGold(10);
    }

}
