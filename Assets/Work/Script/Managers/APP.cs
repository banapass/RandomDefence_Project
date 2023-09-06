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

        UIManager.Instance.Show(UIPath.INTRO, false);
    }
    private void AppSetting()
    {
        Application.targetFrameRate = 60;
    }
    public void EnterInGame()
    {

        UIManager.Instance.Show(UIPath.LOADING, true, () =>
        {
            LoadingManager.Instance.LoadScene("Scene/InGame", OnEnterInGame, "Effector", "Projectile", "Unit","Pooling");
        });


        // StartCoroutine(ChangeScene(() =>
        // {
        //     // UIManager.Instance.Show(UiPath.INGAME, false, OnEnterInGame);
        // }));
    }
    public void EnterIntro()
    {
        UIManager.Instance.Show(UIPath.INTRO, false, () =>
        {

        });
    }
    //private IEnumerator ChangeScene(System.Action _onLoaded)
    //{
    //    var _handle = Addressables.LoadSceneAsync("Scene/Loading", LoadSceneMode.Additive);
    //    yield return _handle.Task;

    //    _onLoaded?.Invoke();
    //}
    private void OnEnterInGame()
    {
        UIManager.Instance.Show(UIPath.INGAME, false, () =>
        {
            ObjectPoolManager.Instance.SetUIParent(UIManager.Instance.UIPropRoot);
            GameManager.Instance.GameStart();
            BoardManager.Instance.Init();
            GameManager.Instance.ChangeGameState(GameState.BreakTime);
            InputController.Instance.Init();
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
