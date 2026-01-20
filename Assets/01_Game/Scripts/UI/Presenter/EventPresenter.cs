using R3;
using UnityEngine;

public class EventPresenter : BasePresenter
{
    // Views
    [SerializeField] private ButtonBinder _eventButtons;
    [SerializeField] private EventView _eventView;

    protected override void Start()
    {
        base.Start();

        var eventDataStore = FindAnyObjectByType<EventDataStore>();

        GameManager.Instance.State
            .Where(state => state == GameState.EVENT)
            .Subscribe( _ =>
            {
                _eventButtons.UnregisterButtonEvent();

                var targetEventID = Random.Range(0, eventDataStore.GetCount);

                var targetEventData = eventDataStore.FindWithId(targetEventID);

                _eventView.SetEventData(targetEventData);

                _eventButtons.RegisterButtonEvent(targetEventData);
            })
            .AddTo(this);
    }
}
