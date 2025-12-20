using DG.Tweening;
using R3;
using R3.Triggers;
using UnityEngine;

[RequireComponent (typeof(ObservableEventTrigger))]
public class ScrollView : MonoBehaviour
{
    // ---------- Field
    [SerializeField] private RectTransform _rect;
    [SerializeField] private float _maxPosY;
    [SerializeField] private float _minPosY;
    [SerializeField] private float _scrollAmount;
    [SerializeField] private float _scrollTime;

    private float _currentPosY;
    private float _targetPosY;
    private float _currentVelocity;
    private bool _canScroll;

    private ObservableEventTrigger _trigger;

    // ---------- UnityMessage
    private void Awake()
    {
        _trigger = GetComponent<ObservableEventTrigger>();
    }
    private void Start()
    {
        _currentPosY = _rect.localPosition.y;
        _targetPosY = _rect.localPosition.y;

        this.UpdateAsObservable()
            .Subscribe( _ =>
            {
                _currentPosY = Mathf.SmoothDamp(
                    _currentPosY,
                    _targetPosY,
                    ref _currentVelocity,
                    _scrollTime);

                _rect.localPosition = new Vector2(
                    _rect.localPosition.x,
                    _currentPosY);
            })
            .AddTo(this);

        OnButtonEntered
            .Subscribe(_ => { _canScroll = true; })
            .AddTo(this);

        OnButtonExited
            .Subscribe(_ => { _canScroll = false; })
            .AddTo(this);
    }

    // ---------- Method
    public void Scroll(float value)
    {
        if (!_canScroll) return;

        _targetPosY -= value * _scrollAmount;

        if (_targetPosY > _maxPosY)
        {
            _targetPosY = _maxPosY;
        }
        else if (_targetPosY < _minPosY)
        {
            _targetPosY = _minPosY;
        }
    }

    /// <summary>
    /// カーソルがボタンの上に乗った時
    /// </summary>
    public Observable<Unit> OnButtonEntered => _trigger
        .OnPointerEnterAsObservable()
        .AsUnitObservable();

    /// <summary>
    /// カーソルがボタンの上から出た時
    /// </summary>
    public Observable<Unit> OnButtonExited => _trigger
        .OnPointerExitAsObservable()
        .AsUnitObservable();
}
