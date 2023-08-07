using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using framework;

public class InputController : Singleton<InputController>
{
    Camera mainCamera;
    List<System.IDisposable> observers;

    private PlacementState placementState = PlacementState.None;
    public static event Action<EmptyTile> OnTriedPlaceNewTile;
    public void Init()
    {
        mainCamera = Camera.main;
        observers = new List<System.IDisposable>();
        var _placementObserver = this.UpdateAsObservable()
                            //FIXME: Break Time 구현 후에 추가
                            // .Where(_ => BoardManager.Instance.IsBreakTime)
                            .Where(_ => placementState == PlacementState.UnitPlacement)
                            .Where(_ => Input.GetMouseButtonDown(0))
                            .Select((_tile) => GetEmptyTile())
                            .Subscribe(_tile => TryPlaceTile(_tile));


        observers.Add(_placementObserver);
    }
    private void OnEnable()
    {
        InGamePage.OnChangePlacementState += OnChagePlacementState;
    }
    private void OnDisable()
    {
        InGamePage.OnChangePlacementState -= OnChagePlacementState;
    }
    private void OnChagePlacementState(PlacementState _state)
    {
        if (placementState == _state) return;
        placementState = _state;
    }
    private EmptyTile GetEmptyTile()
    {
        EmptyTile _result = null;

        Vector2 _mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var _hitInfo = Physics2D.Raycast(_mousePos, Vector3.forward, float.MaxValue);

        if (_hitInfo)
            _result = _hitInfo.collider.GetComponent<EmptyTile>();

        return _result;
    }
    private void TryPlaceTile(EmptyTile _tile)
    {
        if (_tile == null) return;
        if (!_tile.IsPlaceable) return;

        OnTriedPlaceNewTile?.Invoke(_tile);
    }
}
