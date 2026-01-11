using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideUIInOrder : MonoBehaviour
{
    // ---------- SerializeField
    [SerializeField] private List<Image> _slideUIs = new List<Image>();
    [SerializeField] private Vector3 _startPos;
    [SerializeField] private Vector3 _endPos;
    [SerializeField] private float _duration;

    // ---------- Field
    private bool _isMoving;

    // ---------- PrivateMethod
    private int _currentStageNum = 0;

    // ---------- Method
    /// <summary>
    /// 現在表示されているUIをスタート位置と逆方向へスライド
    /// </summary>
    public void SlideIn()
    {
        // アニメーション中なら処理しない
        if (_isMoving) return;

        _isMoving = true;

        // 現在表示されているUIのナンバーを取得
        var currentStageNum = _currentStageNum;

        // 最初のUIの時に実行したら最後のUIに移動
        var nextStageNum = _currentStageNum - 1 < 0 ? _slideUIs.Count - 1 : _currentStageNum - 1;

        // 今表示されているUIを画面外にスライド
        _slideUIs[currentStageNum].rectTransform.DOAnchorPos(
            -_startPos,
            _duration)
            .OnComplete(() => _slideUIs[currentStageNum].gameObject.SetActive(false))
            .SetLink(gameObject);

        // 次表示したいUIをアニメーション開始位置にセット
        _slideUIs[nextStageNum].rectTransform.localPosition = _startPos;

        // 次表示したいUIを表示
        _slideUIs[nextStageNum].gameObject.SetActive(true);

        // 次表示したいUIを対象位置までアニメーション
        _slideUIs[nextStageNum].rectTransform.DOAnchorPos(
            _endPos,
            _duration)
            .OnComplete(() => _isMoving = false)
            .SetLink(gameObject);

        // 現在表示されているUIのナンバーを更新
        _currentStageNum = nextStageNum;
    }

    /// <summary>
    /// 現在表示されているUIをスタートと逆方向へスライド
    /// </summary>
    public void SlideOut()
    {
        // アニメーション中なら処理しない
        if (_isMoving) return;

        _isMoving = true;

        // 現在表示されているUIのナンバーを取得
        var currentStageNum = _currentStageNum;

        // 最後のUIの時に実行したら最初のUIに移動
        var nextStageNum = _currentStageNum + 1 >= _slideUIs.Count ? 0 : _currentStageNum + 1;

        // 今表示されているUIを画面外にスライド
        _slideUIs[currentStageNum].rectTransform.DOAnchorPos(
            _startPos,
            _duration)
            .OnComplete(() => _slideUIs[currentStageNum].gameObject.SetActive(false))
            .SetLink(gameObject);

        // 次表示したいUIをアニメーション開始位置にセット
        _slideUIs[nextStageNum].rectTransform.localPosition = -_startPos;

        // 次表示したいUIを表示
        _slideUIs[nextStageNum].gameObject.SetActive(true);

        // 次表示したいUIを対象位置までアニメーション
        _slideUIs[nextStageNum].rectTransform.DOAnchorPos(
            _endPos,
            _duration)
            .OnComplete(() => _isMoving = false)
            .SetLink(gameObject);

        // 現在表示されているUIのナンバーを更新
        _currentStageNum = nextStageNum;
    }
}
