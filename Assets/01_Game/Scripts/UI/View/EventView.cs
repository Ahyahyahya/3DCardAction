using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _eventName;
    [SerializeField] private Image _eventImage;
    [SerializeField] private TextMeshProUGUI _eventDescription;
    [SerializeField] private List<TextMeshProUGUI> _buttonDescriptions = new();

    public void SetEventData(EventData eventData)
    {
        _eventName.text = eventData.DataName;
        _eventImage.sprite = eventData.EventSprite;
        _eventDescription.text = eventData.EventText;

        for (int i = 0;  i < _buttonDescriptions.Count; i++)
        {
            if (i >= eventData.ButtonEventsList.Count) continue;

            _buttonDescriptions[i].text = eventData.ButtonEventsList[i].EventDescription;
        }
    }
}
