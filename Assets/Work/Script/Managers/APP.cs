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
        AtlasManager.Instance.Init();
        TableManager.Instance.Init();
        MemoryPoolManager.Instance.Init();
        UIManager.Instance.Show(UiPath.INGAME, false);


        GameManager.Instance.ChangeGameState(GameState.BreakTime);
        BoardManager.Instance.Init();
        InputController.Instance.Init();

        ObjectPoolManager.Instance.AddPool<Effector>(ResourceStorage.GetResource<Effector>("Prefab/Effect/DieEffect"), 10, "Die");
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
