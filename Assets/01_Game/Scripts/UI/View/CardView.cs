using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _cardNameTMP;
    [SerializeField] private TextMeshProUGUI _costTMP;
    [SerializeField] private TextMeshProUGUI _cardDescriptionTMP;
    [SerializeField] private TextMeshProUGUI _priceTMP;
    [SerializeField] private Image _cardImage;
    public void SetCardData(CardData cardData)
    {
        _cardNameTMP.text = cardData.DataName;
        _costTMP.text = cardData.Cost.ToString();
        _cardDescriptionTMP.text = cardData.Description;
        _priceTMP.text = cardData.Price.ToString();
        _cardImage.sprite = cardData.Sprite;
    }
}
