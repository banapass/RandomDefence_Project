using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using framework;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class APP : Singleton<APP>
{
    private void Start()
    {
        AppSetting();
        Init();
    }
    public async void Init()
    {
        await ResourcesLoad();

        AtlasManager.Instance.Init();
        TableManager.Instance.Init();
        MemoryPoolManager.Instance.Init();

        UIManager.Instance.Show(UiPath.INTRO, false);
    }
    private void AppSetting()
    {
        Application.targetFrameRate = 60;
    }
    public void EnterInGame()
    {

        UIManager.Instance.Show(UiPath.LOADING, true, () =>
        {
            LoadingManager.Instance.LoadScene("Scene/InGame", OnEnterInGame, "Effector", "Projectile", "Unit");
        });

        // StartCoroutine(ChangeScene(() =>
        // {
        //     // UIManager.Instance.Show(UiPath.INGAME, false, OnEnterInGame);
        // }));
    }
    private IEnumerator ChangeScene(System.Action _onLoaded)
    {
        var _handle = Addressables.LoadSceneAsync("Scene/Loading", LoadSceneMode.Additive);
        yield return _handle.Task;

        _onLoaded?.Invoke();
    }
    private void OnEnterInGame()
    {
        UIManager.Instance.Show(UiPath.INGAME, false, () =>
        {
            ObjectPoolManager.Instance.SetUIParent(UIManager.Instance.UIParent);
            GameManager.Instance.GameStart();
            BoardManager.Instance.Init();
            GameManager.Instance.ChangeGameState(GameState.BreakTime);
            InputController.Instance.Init();

            // ResourceStorage.GetComponentAsset<Slider>(Constants.MONSTER_HPBAR, _res =>
            // {
            //     ObjectPoolManager.Instance.AddPool<Slider>(_res, 10, Constants.MONSTER_HPBAR, UIManager.Instance.CurrentPage.transform);
            // });


        });
    }
    public async Task ResourcesLoad()
    {
        ResourceStorage.ClearResource();

        await ResourceStorage.LoadObjectByLabel<TextAsset>("Data");
        // await ResourceStorage.LoadComponentsByLabel<ProjectileBase>("Projectile", false, (_key, _comp) =>
        // {
        //     ObjectPoolManager.Instance.AddPool<ProjectileBase>(_comp as ProjectileBase, 2, _key);
        // });

        // await ResourceStorage.LoadComponentsByLabel<Effector>("Effector", false, (_key, _comp) =>
        // {
        //     var _component = _comp.gameObject.GetComponent<Effector>();
        //     ObjectPoolManager.Instance.AddPool<Effector>(_component, 10, _key);
        // });


    }

}
