//  TouchEventHandler.cs
//  http://kan-kikuchi.hatenablog.com/entry/uGUI_TouchEventHandler
//
//  Created by kan.kikuchi on 2016.04.20.

using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// タッチ判定をハンドリングするクラス
/// </summary>
public class TouchEventHandler : SingletonMonoBehaviour<TouchEventHandler>, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    //タップ中
    private bool _isPressing = false;
    public bool IsPressing
    {
        get { return _isPressing; }
    }

    //ドラック中か
    private bool _isDragging = false;
    public bool IsDragging
    {
        get { return _isDragging; }
    }

    //ピンチ中かのフラグ
    private bool _isPinching = false;
    public bool IsPinching
    {
        get { return _isPinching; }
    }

    //全フレームでのドラック位置(ワールド座標)
    private Vector3 _beforeTapWorldPoint;

    //ピンチ開始時の指の距離
    private float _beforeDistanceOfPinch;

    //タップ関係
    public event Action<bool> onPress = delegate { };
    public event Action onBeginPress = delegate { };
    public event Action onEndPress = delegate { };

    //ドラッグ
    public event Action<Vector2> onDrag = delegate { };
    public event Action<Vector3> onDragIn3D = delegate { };
    public event Action onBeginDrag = delegate { };
    public event Action onEndDrag = delegate { };

    //ビンチ
    public event Action<float> onPinch = delegate { };
    public event Action onBeginPinch = delegate { };
    public event Action onEndPinch = delegate { };

    //ドラッグしている指のデータ
    private Dictionary<int, PointerEventData> _draggingDataDict = new Dictionary<int, PointerEventData>();

    //=================================================================================
    //タップ
    //=================================================================================

    /// <summary>
    /// タップ開始時
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressing = true;

        onBeginPress();
        onPress(true);
    }

    /// <summary>
    /// タップ終了時
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressing = false;

        onEndPress();
        onPress(false);

        //ピンチ終了イベント実行
        if (_isPinching && _draggingDataDict.Count < 2)
        {
            _isPinching = false;
            onEndPinch();
        }
    }

    //=================================================================================
    //ドラッグ
    //=================================================================================

    /// <summary>
    /// ドラッグ開始時
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_isDragging)
        {
            _beforeTapWorldPoint = GetTapWorldPoint();
        }

        _isDragging = true;
        onBeginDrag();

        //ドラッグデータ追加
        _draggingDataDict[eventData.pointerId] = eventData;
    }

    /// <summary>
    /// ドラッグ終了時
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;
        onEndDrag();

        //ドラッグデータ削除
        if (_draggingDataDict.ContainsKey(eventData.pointerId))
        {
            _draggingDataDict.Remove(eventData.pointerId);
        }
    }

    /// <summary>
    /// ドラッグ中
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging)
        {
            OnBeginDrag(eventData);
            return;
        }

        //ドラッグデータ更新
        _draggingDataDict[eventData.pointerId] = eventData;

        //2本以上ドラッグデータがある時はピンチ
        if (_draggingDataDict.Count >= 2)
        {
            //ドラック中だった場合は終了する
            if (_isDragging)
            {
                _isDragging = false;
                onEndDrag();
            }

            OnPinch();
        }
        //指が一本だけの時だけドラッグ(PC上では0本)
        else if (Input.touches.Length <= 1)
        {

            //ピンチの状態からすぐ移ってきた場合はピンチの終了処理、ドラッグの開始処理をする
            if (_isPinching)
            {
                _isDragging = false;
                _isPinching = false;
                onEndPinch();
                OnBeginDrag(eventData);
            }

            onDrag(eventData.delta);
            OnDragInWorldPoint();
        }
    }

    /// <summary>
    /// ドラッグ中(座標をワールド座標で返す)
    /// </summary>
    public void OnDragInWorldPoint()
    {
        Vector3 tapWorldPoint = GetTapWorldPoint();
        onDragIn3D(tapWorldPoint - _beforeTapWorldPoint);
        _beforeTapWorldPoint = tapWorldPoint;
    }

    //タップしている場所をワールド座標で取得
    private Vector3 GetTapWorldPoint()
    {
        //タップ位置を画面内の座標に変換
        Vector2 screenPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(),
          new Vector2(Input.mousePosition.x, Input.mousePosition.y),
          null,
          out screenPos
        );

        //ワールド座標に変換
        Vector3 tapWorldPoint = Camera.main.ScreenToWorldPoint(
          new Vector3(screenPos.x, screenPos.y, -Camera.main.transform.position.z)
        );

        return tapWorldPoint;
    }

    //=================================================================================
    //ピンチ
    //=================================================================================

    //ピンチ中
    private void OnPinch()
    {

        //最初と2本目のタッチ情報取得
        Vector2 firstTouch = Vector2.zero, secondTouch = Vector2.zero;
        int count = 0;

        foreach (var draggingData in _draggingDataDict)
        {
            if (count == 0)
            {
                firstTouch = draggingData.Value.position;
                count = 1;
            }
            else if (count == 1)
            {
                secondTouch = draggingData.Value.position;
                break;
            }
        }

        //ピンチの幅を取得
        float distanceOfPinch = Vector2.Distance(firstTouch, secondTouch);

        //ピンチ開始
        if (!_isPinching)
        {
            _isPinching = true;
            _beforeDistanceOfPinch = distanceOfPinch;

            onBeginPinch();
            return;
        }

        //現在地の座標差も算出し、座標差からピンチの倍率を計算
        float pinchiDiff = distanceOfPinch - _beforeDistanceOfPinch;
        onPinch(pinchiDiff);


        _beforeDistanceOfPinch = distanceOfPinch;
    }

}