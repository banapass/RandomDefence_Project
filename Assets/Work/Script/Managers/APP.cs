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


        GameManager.Instance.GameStart();
        GameManager.Instance.ChangeGameState(GameState.BreakTime);
        BoardManager.Instance.Init();
        InputController.Instance.Init();

        ObjectPoolManager.Instance.AddPool<Effector>(ResourceStorage.GetResource<Effector>("Prefab/Effect/DieEffect"), 10, "Die");
    }
}
