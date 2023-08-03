using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using framework;

public class APP : Singleton<APP>
{
    private void Start()
    {
        Init();       
    }
    public void Init()
    {
        Tile _tile = ResourceStorage.GetResource<Tile>("Prefab/Tile");

        UIManager.Instance.Show("UI/Page/Intro", true);
    }
}
