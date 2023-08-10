using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Unit : MonoBehaviour
{
    protected UnitInfo unitInfo;
    protected float currentCoolTime;

    public void Init(UnitInfo _unitInfo)
    {
        unitInfo = _unitInfo;
        currentCoolTime = unitInfo.coolTime;
    }
    public void OnAttack()
    {

    }
    public void Cooldown(float _deltaTime)
    {
        if (IsReadyToAttack()) return;
        currentCoolTime -= _deltaTime;
    }
    public bool IsReadyToAttack()
    {
        return currentCoolTime <= 0;
    }
    public void ResetCoolTime()
    {
        currentCoolTime = unitInfo.coolTime;
    }
}
