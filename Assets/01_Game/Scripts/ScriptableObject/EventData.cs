using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "ScriptableObject/Data/Event")]
public class EventData : BaseData
{
    [SerializeField] private Sprite _eventSprite;
    [SerializeField] private string _eventText;
    [SerializeField] private List<BaseEventList> _buttonEventsList = new();

    public Sprite EventSprite => _eventSprite;
    public string EventText => _eventText;
    public List<BaseEventList> ButtonEventsList => _buttonEventsList;
}

[System.Serializable]
public class BaseEventList : ListWrapper<BaseEvent>
{
    [SerializeField] private string _eventDescription;

    public string EventDescription => _eventDescription;
}
