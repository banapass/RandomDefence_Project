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

    public static event Action<PlacementState> OnChangePlacementState;
    public override void OnOpen()
    {
        unitPlacementBtn.OnClickAsObservable()
        .Subscribe(_observer => OnChangePlacementState?.Invoke(PlacementState.UnitPlacement));
    }
}
