using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using framework;
using UnityEngine.AddressableAssets;

public class APP : Singleton<APP>
{
    [SerializeField] float angle;
    [SerializeField] int count;
    private void Start()
    {
        Init();
    }
    public async void Init()
    {
        await ResourcesLoad();

        AtlasManager.Instance.Init();
        TableManager.Instance.Init();
        MemoryPoolManager.Instance.Init();
        UIManager.Instance.Show(UiPath.INGAME, false);


        GameManager.Instance.GameStart();
        BoardManager.Instance.Init();
        GameManager.Instance.ChangeGameState(GameState.BreakTime);
        InputController.Instance.Init();

    }

    public async Task ResourcesLoad()
    {
        ResourceStorage.ClearResource();

        await ResourceStorage.LoadComponentsByLabel<ProjectileBase>("Projectile", false, (_key, _comp) =>
        {
            // var _component = _comp.gameObject.GetComponent<ProjectileBase>();
            ObjectPoolManager.Instance.AddPool<ProjectileBase>(_comp as ProjectileBase, 2, _key);
        });

        await ResourceStorage.LoadComponentsByLabel<Effector>("Effector", false, (_key, _comp) =>
        {
            var _component = _comp.gameObject.GetComponent<Effector>();
            ObjectPoolManager.Instance.AddPool<Effector>(_component, 10, _key);
        });

        await ResourceStorage.LoadObjectByLabel<TextAsset>("Data");
    }

}
