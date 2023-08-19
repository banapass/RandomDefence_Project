using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using framework;
using TMPro;

public class InGamePage : BaseUi
{
    [Header("Button")]
    [SerializeField] Button unitPlacementBtn;
    [SerializeField] Button unitBtn;
    [SerializeField] Button nextRoundBtn;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI gold_txt;

    [SerializeField] Heart[] hearts;


    private const string GOLD_FORMAT = "{0} G";
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
        GameManager.OnLifeDamaged += OnLifeDamaged;
        GameManager.OnChangedGold += UpdateCost;
    }
    private void OnDisable()
    {
        GameManager.OnChangedGameState -= OnChangedGameState;
        GameManager.OnLifeDamaged -= OnLifeDamaged;
        GameManager.OnChangedGold -= UpdateCost;
    }

    private void OnChangedGameState(GameState _changedStage)
    {
        bool _isBreakTime = _changedStage == GameState.BreakTime;

        nextRoundBtn.enabled = _isBreakTime;
    }
    private void OnLifeDamaged(int _currLife)
    {
        // if (_currLife <= 0)
        // {
        //     hearts[0].TakeDamage();
        //     return;
        // }

        for (int i = hearts.Length - 1; i >= 0; i--)
        {
            bool _isDamagedHeart = i >= _currLife;

            if (_isDamagedHeart)
                hearts[i].TakeDamage();
        }
    }

    private void UpdateCost(int _currCost)
    {
        gold_txt.text = string.Format(GOLD_FORMAT, _currCost);
    }
}
