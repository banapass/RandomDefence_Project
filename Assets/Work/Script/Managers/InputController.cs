using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using framework;

public class InputController : Singleton<InputController>
{
    private Camera mainCamera;
    private List<System.IDisposable> observers;
    private LayerMask tileLayer, unitPlacementLayer;

    private PlacementState placementState = PlacementState.None;
    public static event Action<EmptyTile> OnTriedPlaceNewTile;
    public void Init()
    {

        mainCamera = Camera.main;
        tileLayer = 1 << LayerMask.NameToLayer("EmptyTile");
        unitPlacementLayer = 1 << LayerMask.NameToLayer("UnitPlacement");

        observers = new List<System.IDisposable>();
        var _placementObserver = this.UpdateAsObservable()
                            .Where(_ => GameManager.Instance.IsBreakTime)
                            .Where(_ => placementState == PlacementState.UnitPlacement)
                            .Where(_ => Input.GetMouseButtonDown(0))
                            .Select(_tile => GetEmptyTile())
                            .Subscribe(_tile => TryPlaceTile(_tile));

        var _unitPlaceObserver = this.UpdateAsObservable()
                                 .Where(_ => placementState == PlacementState.Unit)
                                 .Where(_ => Input.GetMouseButtonDown(0))
                                 .Select(_unitTile => GetUnitPlacementTile())
                                 .Subscribe(_unitTile => TryPlaceNewUnit(_unitTile));



        observers.Add(_placementObserver);
        observers.Add(_unitPlaceObserver);
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
        Debug.Log(_state);
        placementState = _state;
    }
    private EmptyTile GetEmptyTile()
    {
        EmptyTile _result = null;

        Vector2 _mousePos = GetMousePosition();
        var _hitInfo = Physics2D.Raycast(_mousePos, Vector3.forward, float.MaxValue, tileLayer);

        if (_hitInfo)
            _result = _hitInfo.collider.GetComponent<EmptyTile>();

        return _result;
    }
    private UnitPlacementTile GetUnitPlacementTile()
    {
        UnitPlacementTile _result = null;


        Vector2 _mousePos = GetMousePosition();
        var _hitInfo = Physics2D.Raycast(_mousePos, Vector3.forward, float.MaxValue, unitPlacementLayer);

        if (_hitInfo)
            _result = _hitInfo.collider.GetComponent<UnitPlacementTile>();


        return _result;
    }
    private void TryPlaceNewUnit(UnitPlacementTile _tile)
    {
        if (_tile == null) return;
        if (_tile.HasUnit) return;

        UnitInfo _selectedUnit = TableManager.Instance.GetRandomUnitInfo();

        Unit _rawUnit = ResourceStorage.GetResource<Unit>("Prefab/Unit");
        Unit _instUnit = Instantiate(_rawUnit);
        _instUnit.Init(_selectedUnit);
        _tile.Init(_instUnit);
    }
    private Vector3 GetMousePosition()
    {
        Vector3 _result = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        return _result;
    }
    private void TryPlaceTile(EmptyTile _tile)
    {
        if (_tile == null) return;
        if (!_tile.IsPlaceable) return;

        OnTriedPlaceNewTile?.Invoke(_tile);
    }
}
