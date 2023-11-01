using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using framework;


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
        UIManager.Instance.Init();
        AudioManager.Instance.Init();

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
            LoadingManager.Instance.LoadScene("Scene/InGame", OnEnterInGame, "Effector", "Projectile", "Unit", "Pooling");
        });
    }
    public void EnterIntro()
    {
        UIManager.Instance.Show(UIPath.INTRO, false, () =>
        {

        });
    }
    private void OnEnterInGame()
    {
        UIManager.Instance.Show(UIPath.INGAME, false, () =>
        {
            ObjectPoolManager.Instance.SetUIParent(UIManager.Instance.UIPropRoot);
            GameManager.Instance.GameStart();
            BoardManager.Instance.Init();
            GameManager.Instance.ChangeGameState(GameState.BreakTime);
            InputController.Instance.Init();

            AudioManager.Instance.PlayMusic(framework.Audio.Music.InGameBGM);
        });
    }
    public async Task ResourcesLoad()
    {
        ResourceStorage.ClearResource();

        await ResourceStorage.LoadObjectByLabel<TextAsset>("Data");
    }

}
