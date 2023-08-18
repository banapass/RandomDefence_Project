using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Heart : MonoBehaviour
{
    private Image heart_img;
    private bool isDamaged = false;

    private const string FULL_HEART = "heart-full";
    private const string EMPTY_HEART = "heart-empty";

    private void Awake()
    {
        heart_img = GetComponent<Image>();
        isDamaged = false;
    }
    public void TakeDamage()
    {
        if (isDamaged) return;


        Sequence _seq = DOTween.Sequence();

        var _startTween = transform.DOScale(0.25f, 0.40f)
                        .SetEase(Ease.InBounce);

        var _endTween = transform.DOScale(1, 0.25f)
        .SetEase(Ease.OutBounce);

        _seq.Append(_startTween);
        _seq.Append(_endTween);
        _seq.OnComplete(() => ChangeHeartSprite(false));
        _seq.Play();

        isDamaged = true;
    }

    private void ChangeHeartSprite(bool _onOff)
    {
        string _targetHeart = _onOff ? FULL_HEART : EMPTY_HEART;
        heart_img.sprite = AtlasManager.Instance.GetSprite(_targetHeart);
    }


}
