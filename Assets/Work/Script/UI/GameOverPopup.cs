using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using framework;
using DG.Tweening;
using UniRx;
using UnityEngine.SceneManagement;

public class GameOverPopup : BaseUi
{
    [SerializeField] RectTransform popupRect;

    [Header("Button")]
    [SerializeField] Button home_btn;
    public override void OnOpen()
    {

        home_btn.OnClickAsObservable()
                .Subscribe(_observer =>
                {
                    LoadingManager.Instance.LoadScene("Scene/Intro", () =>
                    {
                        UIManager.Instance.Show(UIPath.INTRO, false);
                    });
                });

        float _halfWidth = Screen.width * 0.5f;
        float _halfHeight = Screen.height * 0.5f;
        float _halfPopupHeight = popupRect.sizeDelta.y * 0.5f;


        popupRect.DOMove(new Vector2(_halfWidth, _halfHeight + _halfPopupHeight), 0.5f)
                 .SetEase(Ease.OutBack);
    }
}
