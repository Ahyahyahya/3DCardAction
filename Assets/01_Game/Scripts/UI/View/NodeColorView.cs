using UnityEngine;
using UnityEngine.UI;

public class NodeColorView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _activeColor;
    [SerializeField] private Color _inactiveColor;

    private bool _isSelected;

    public void ChangeToSelectedColor()
    {
        _image.color = _selectedColor;

        _isSelected = true;
    }

    public void ChangeToActiveColor()
    {
        if (_isSelected) return;

        _image.color = _activeColor;
    }

    public void ChangeToInactiveColor()
    {
        if (_isSelected) return;

        _image.color = _inactiveColor;
    }
}
