using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlideCardsView : MonoBehaviour
{
    // ---------- SerializeField
    [SerializeField] private List<GameObject> _cards = new();
    [SerializeField] private float _duration;

    [SerializeField] private List<Vector2> _cardPositions = new();

    private bool _isSliding = false;

    private int _useCardNum = 1;

    // ---------- UnityMessage
    private void Start()
    {
        foreach (var card in _cards)
        {
            var cardRect = card.GetComponent<RectTransform>();

            _cardPositions.Add(cardRect.transform.localPosition);
        }
    }

    // ---------- Method
    public void SlideIn()
    {
        if (_isSliding) return;

        _isSliding = true;

        for (int i = 0; i < _cards.Count; i++)
        {
            var card = _cards[i];

            var cardRect = card.GetComponent<RectTransform>();

            var targetIndex = (i + 1) % _cards.Count;

            cardRect.DOAnchorPos(
                _cardPositions[targetIndex],
                _duration)
                .OnComplete(() => _isSliding = false)
                .SetLink(gameObject);
        }
    }

    public void SlideOut()
    {
        if (_isSliding) return;

        _isSliding = true;

        for (int i = 0; i < _cards.Count; i++)
        {
            var card = _cards[i];

            var cardRect = card.GetComponent<RectTransform>();

            int targetIndex = (_cards.Count + i - 1) % _cards.Count;

            cardRect.DOAnchorPos(
                _cardPositions[targetIndex],
                _duration)
                .OnComplete(() => _isSliding = false)
                .SetLink(gameObject);
        }
    }

    public void Slide(int curCardNum)
    {
        Debug.Log(curCardNum);

        if (_isSliding) return;

        _isSliding = true;

        for (int i = 0; i < _cards.Count; i++)
        {
            var card = _cards[i];

            var cardRect = card.GetComponent<RectTransform>();

            var targetIndex = (_cards.Count + i - curCardNum + _useCardNum) % _cards.Count;

            cardRect.DOAnchorPos(
                _cardPositions[targetIndex],
                _duration)
                .OnComplete(() => _isSliding = false)
                .SetLink(gameObject);
        }
    }
}
