//  CanvasFader.cs
//  http://kan-kikuchi.hatenablog.com/entry/CanvasFader
//
//  Created by kan.kikuchi on 2016.05.11.

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
/// <summary>
/// キャンバスをフェードするクラス
/// </summary>
public class CanvasFader : MonoBehaviour
{
    //フェード用のキャンバス
    [SerializeField] private CanvasGroup _canvasGroup;

    //フェード時間
    [SerializeField]
    private float _duration;

    // ---------- UnityMessage
    private void Start()
    {
        if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    // ---------- Method
    /// <summary>
    /// フェードを開始する
    /// </summary>
    public void FadeIn()
    {
        _canvasGroup.DOFade(
            1,
            _duration)
            .SetLink(gameObject);
    }

    /// <summary>
    /// フェードを開始する
    /// </summary>
    public void FadeOut()
    {
        _canvasGroup.DOFade(
            0,
            _duration)
            .SetLink(gameObject);
    }
}