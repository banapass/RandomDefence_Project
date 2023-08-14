using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using framework;

public class InGamePage : BaseUi
{
    [SerializeField] Button unitPlacementBtn;
    [SerializeField] Button unitBtn;
    [SerializeField] Button nextRoundBtn;

    public static event Action<PlacementState> OnChangePlacementState;

    public override void OnOpen()
    {
        unitPlacementBtn.OnClickAsObservable()
        .Subscribe(_observer => OnChangePlacementState?.Invoke(PlacementState.UnitPlacement));

        nextRoundBtn.OnClickAsObservable()
        .Subscribe(_observer => WaveManager.Instance.StartNextRound());

        unitBtn.OnClickAsObservable()
        .Subscribe(_observer => OnChangePlacementState?.Invoke(PlacementState.Unit));

    }

    private void OnEnable()
    {
        GameManager.OnChangedGameState += OnChangedGameState;
    }
    private void OnDisable()
    {
        GameManager.OnChangedGameState -= OnChangedGameState;
    }

    private void OnChangedGameState(GameState _changedStage)
    {
        bool _isBreakTime = _changedStage == GameState.BreakTime;

        nextRoundBtn.enabled = _isBreakTime;


    }
}
