using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SliderAnimation : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public void SetValueWithAnimation(float value)
    {
        // アニメーションしながらSliderを動かす
        DOTween.To(() => _slider.value,
            n => _slider.value = n,
            value,
            duration: 1.0f);
    }

    public void SetValue(float value)
    {
        _slider.value = value;
    }
}
