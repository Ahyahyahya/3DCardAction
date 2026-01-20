using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBinder : MonoBehaviour
{
    // Field
    [SerializeField] private List<GameObject> _buttons = new();

    public void RegisterButtonEvent(EventData eventData)
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            if (i >= eventData.ButtonEventsList.Count)
            {
                _buttons[i].SetActive(false);
                continue;
            }

            _buttons[i].SetActive(true);

            foreach (var targetEvent in eventData.ButtonEventsList[i].List)
            {
                _buttons[i]
                    .GetComponent<Button>()
                    .onClick
                    .AddListener(targetEvent.Execute);
            }
        }
    }

    public void UnregisterButtonEvent()
    {
        foreach (var button in _buttons)
        {
            button
                .GetComponent<Button>()
                .onClick
                .RemoveAllListeners();
        }
    }
}
