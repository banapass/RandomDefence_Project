using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextEffector : Effector
{
    private TextMeshPro textMesh;
    private Sequence sequence;


    private const string DAMAGE_FORMET = "-{0}";
    private const float DEFAULT_SCALE = 0.14f;

    private void Awake()
    {
        TryGetComponent<TextMeshPro>(out textMesh);
        SetSequence();
    }

    public void Play(float _damage)
    {
        textMesh.text = string.Format(DAMAGE_FORMET, _damage);

        // if (sequence == null) SetSequence();
        // sequence?.Play();

        // transform.localScale = Vector2.one * DEFAULT_SCALE;
        // transform
        // .DOScale(DEFAULT_SCALE * 1.5f, 0.5f)
        // .SetEase(Ease.OutElastic)
        // .OnComplete(() => ObjectPoolManager.Instance.ReturnParts(this, ObjectID));

        transform.DOMove(transform.position + Vector3.up * 1.5f, 1f)
        .SetEase(Ease.OutCirc)
        .OnComplete(() => ReturnPool());


    }
    private void SetSequence()
    {
        // sequence = DOTween.Sequence();
        // // var _moveTween = transform
        // //                 .DOMove(transform.position + Vector3.up * 1.2f, 0.5f)
        // //                 .SetEase(Ease.OutBack);




        // sequence.Append(_scaleTween);
        // sequence.OnStart(() =>
        // {
        //     transform.localScale = Vector2.one * DEFAULT_SCALE;
        // });
        // sequence.OnComplete(() =>
        // {
        //     ObjectPoolManager.Instance.ReturnParts(this, ObjectID);
        // });
        // sequence.SetAutoKill(false);
    }
}
