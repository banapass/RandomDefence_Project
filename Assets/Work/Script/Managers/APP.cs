using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using framework;

public class APP : Singleton<APP>
{
    [SerializeField] float angle;
    [SerializeField] int count;
    private void Start()
    {
        Init();
    }
    public void Init()
    {
        InputController.Instance.Init();
        UIManager.Instance.Show(UiPath.INGAME, false);
        TableManager.Instance.Init();
        MemoryPoolManager.Instance.Init();


        GameManager.Instance.ChangeGameState(GameState.BreakTime);
        BoardManager.Instance.Init();
    }

    private void OnEnable()
    {
        Monster.OnTakeDamage += OnTakeDamage;
    }

    private void OnTakeDamage(MonsterHitInfo _hitInfo)
    {
        Debug.Log($"TakeDamage : {_hitInfo.takeDamage} : HitPosition : {_hitInfo.hitPosition}");
    }
}
