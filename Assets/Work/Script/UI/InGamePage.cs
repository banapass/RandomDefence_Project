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
    [SerializeField] Button sellBtn;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI gold_txt;
    [SerializeField] TextMeshProUGUI round_txt;

    [Header("Tweener")]
    [SerializeField] Tweener tweener_startBtn;

    [SerializeField] Heart[] hearts;


    private const string GOLD_FORMAT = "{0} G";
    private const string ROUND_FORMAT = "Round {0}";
    public static event Action<PlacementState> OnChangePlacementState;

    public override void OnOpen()
    {
        unitPlacementBtn.OnClickAsObservable()
        .Subscribe(_observer => OnChangePlacementState?.Invoke(PlacementState.UnitPlacement));

        nextRoundBtn.OnClickAsObservable()
        .Subscribe(_observer => WaveManager.Instance.StartNextRound());

        unitBtn.OnClickAsObservable()
        .Subscribe(_observer => OnChangePlacementState?.Invoke(PlacementState.Unit));

        sellBtn.OnClickAsObservable()
        .Subscribe(_observer => OnChangePlacementState?.Invoke(PlacementState.Sell));


        // UIManager.Instance.Show(UIPath.GAMEOVER, true);
    }

    private void OnEnable()
    {
        GameManager.OnChangedGameState += OnChangedGameState;
        GameManager.OnLifeDamaged += OnLifeDamaged;
        GameManager.OnChangedGold += UpdateCost;
        WaveManager.OnChangedRound += OnChangedRound;
    }
    private void OnDisable()
    {
        GameManager.OnChangedGameState -= OnChangedGameState;
        GameManager.OnLifeDamaged -= OnLifeDamaged;
        GameManager.OnChangedGold -= UpdateCost;
        WaveManager.OnChangedRound -= OnChangedRound;
    }

    private bool isTest = true;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTest)
                tweener_startBtn.Show();
            else
                tweener_startBtn.Hide();

            isTest = !isTest;
        }
    }

    private void OnChangedRound(int _currRound)
    {
        round_txt.text = string.Format(ROUND_FORMAT, _currRound);
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
