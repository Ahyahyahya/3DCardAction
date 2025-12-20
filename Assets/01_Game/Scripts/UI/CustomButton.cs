using R3;
using R3.Triggers;
using UnityEngine;

[RequireComponent(typeof(ObservableEventTrigger))]
public class CustomButton : MonoBehaviour
{
    // ---------- Field
    private ObservableEventTrigger _trigger;
    private ObservableEventTrigger Trigger
    {
        get
        {
            if (_trigger == null)
            {
                _trigger = GetComponent<ObservableEventTrigger>();
            }

            return _trigger;
        }
    }


    // ボタンのアクティブ状態を保持するReactiveProperty
    private readonly ReactiveProperty<bool> _isActived = new(true);
    public ReadOnlyReactiveProperty<bool> IsActived => _isActived;

    // ------------------------------ UnityMessage
    protected virtual void Awake()
    {
        _isActived.AddTo(this);
    }

    // ------------------------------ PublicMethod
    /// <summary>
    /// ボタンクリック時
    /// </summary>
    public Observable<Unit> OnButtonClicked => Trigger
        .OnPointerClickAsObservable()
        .AsUnitObservable()
        .Where(_ => _isActived.Value);

    /// <summary>
    /// ボタン押した時
    /// </summary>
    public Observable<Unit> OnButtonPressed => Trigger
        .OnPointerDownAsObservable()
        .AsUnitObservable()
        .Where(_ => _isActived.Value);

    /// <summary>
    /// ボタン離した時
    /// </summary>
    public Observable<Unit> OnButtonReleased => Trigger
        .OnPointerUpAsObservable()
        .AsUnitObservable()
        .Where (_ => _isActived.Value);

    /// <summary>
    /// カーソルがボタンの上に乗った時
    /// </summary>
    public Observable<Unit> OnButtonEntered => Trigger
        .OnPointerEnterAsObservable()
        .AsUnitObservable()
        .Where(_ => _isActived.Value);

    /// <summary>
    /// カーソルがボタンの上から出た時
    /// </summary>
    public Observable<Unit> OnButtonExited => Trigger
        .OnPointerExitAsObservable()
        .AsUnitObservable()
        .Where(_ => _isActived.Value);

    public void Active() => _isActived.Value = true;

    public void Inactive() => _isActived.Value = false;
}
