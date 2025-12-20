using DG.Tweening;
using UnityEngine;

public class ScalingView : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private float _targetScale;
    [SerializeField] private float _scalingTime;
    [SerializeField] private Ease _ease;

    public void ScalingOnce()
    {
        _rect.DOScale(_targetScale, _scalingTime)
            .SetEase(_ease)
            .SetLink(gameObject);
    }

    public void ScalingLoop()
    {
        _rect.DOScale(_targetScale, _scalingTime)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(_ease)
            .SetLink(gameObject);
    }

    public void ScalingStop()
    {
        _rect.DOKill();
        _rect.localScale = Vector3.one;
    }
}
