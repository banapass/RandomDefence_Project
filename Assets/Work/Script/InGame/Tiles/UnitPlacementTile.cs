using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlacementTile : BaseTile
{
    public override bool IsWalkable => false;
    private Unit unit;

    public void Init(Unit _unit)
    {
        if (unit != null) return;
        unit = _unit;
        unit.transform.position = transform.position;
    }

    private void Update()
    {
        if (unit == null) return;

        unit.Cooldown(Time.deltaTime);

        if (unit.IsReadyToAttack())
        {
            if (unit.OnAttack())
                unit.ResetCoolTime();
        }
    }

}
