using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using framework;
using Utility;

public class GameClearPopup : BaseUi
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


        StartPositioning();
        popupRect.DOAnchorPos(Vector2.zero, 0.5f)
                 .SetEase(Ease.OutBack);
    }

    private void StartPositioning()
    {
        Vector2 _bottomPos = UIUtility.GetCanvasPosition(PositionType.Bottom, popupRect, new Vector2(Screen.width, Screen.height));
        popupRect.anchoredPosition = new Vector2(_bottomPos.x, _bottomPos.y);
    }
}
