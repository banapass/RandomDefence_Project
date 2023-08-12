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

    }
}
