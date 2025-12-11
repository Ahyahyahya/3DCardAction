using R3;
using R3.Triggers;
using UnityEngine;

[RequireComponent(typeof(ObservableEventTrigger))]
public class CustomButton : MonoBehaviour
{
    // ---------- Field
    private ObservableEventTrigger _trigger;

    // ボタンのアクティブ状態を保持するReactiveProperty
    private readonly ReactiveProperty<bool> _isActived = new(true);
    public ReadOnlyReactiveProperty<bool> IsActived => _isActived;

    // ------------------------------ UnityMessage
    protected virtual void Awake()
    {
        _trigger = GetComponent<ObservableEventTrigger>();
        _isActived.AddTo(this);
    }

    // ------------------------------ PublicMethod
    /// <summary>
    /// ボタンクリック時
    /// </summary>
    public Observable<Unit> OnButtonClicked => _trigger
        .OnPointerClickAsObservable()
        .AsUnitObservable()
        .Where(_ => _isActived.Value);

    /// <summary>
    /// ボタン押した時
    /// </summary>
    public Observable<Unit> OnButtonPressed => _trigger
        .OnPointerDownAsObservable()
        .AsUnitObservable()
        .Where(_ => _isActived.Value);

    /// <summary>
    /// ボタン離した時
    /// </summary>
    public Observable<Unit> OnButtonReleased => _trigger
        .OnPointerUpAsObservable()
        .AsUnitObservable()
        .Where (_ => _isActived.Value);

    /// <summary>
    /// カーソルがボタンの上に乗った時
    /// </summary>
    public Observable<Unit> OnButtonEntered => _trigger
        .OnPointerEnterAsObservable()
        .AsUnitObservable()
        .Where(_ => _isActived.Value);

    /// <summary>
    /// カーソルがボタンの上から出た時
    /// </summary>
    public Observable<Unit> OnButtonExited => _trigger
        .OnPointerExitAsObservable()
        .AsUnitObservable()
        .Where(_ => _isActived.Value);
}
