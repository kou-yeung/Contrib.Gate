//  Joystick.cs
//  http://kan-kikuchi.hatenablog.com/entry/uGUI_Joystick_2
//
//  Created by kan.kikuchi on 2016.07.19.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ジョイスティック
/// </summary>
public class Joystick : MonoBehaviour
{

    //実際に動くスティック部分
    [SerializeField]
    [Header("実際に動くスティック部分(自動設定)")]
    private GameObject _stick = null;
    private const string STICK_NAME = "Stick";

    //スティックが動く範囲の半径
    [SerializeField]
    [Header("スティックが動く範囲の半径")]
    private float _radius = 100;

    //現在地(x,y共に値が-1~1の範囲になる)
    [SerializeField]
    [Header("現在地(自動更新)")]
    private Vector2 _position = Vector2.zero;
    public Vector2 Position { get { return _position; } }

    //スティックの位置(Setter)
    private Vector3 _stickPosition
    {
        set
        {
            _stick.transform.localPosition = value;
            _position = new Vector2(
              _stick.transform.localPosition.x / _radius,
              _stick.transform.localPosition.y / _radius
            );
        }
    }

    //初期化されているか
    public bool IsInitialized
    {
        get
        {
            //スティックが設定されていれば初期化済み
            if (_stick != null)
            {
                return true;
            }

            //スティックが子にあるか検索、あれば初期化済み
            if (transform.Find(STICK_NAME) != null)
            {
                _stick = transform.Find(STICK_NAME).gameObject;
                return true;
            }

            return false;
        }
    }

    //=================================================================================
    //初期化
    //=================================================================================

    private void Awake()
    {
        //必要なら初期化する
        InitIfNeeded();

        //スティックのImageにタッチ判定を取られないようにraycastTargetをfalseに
        _stick.GetComponent<Image>().raycastTarget = false;

        //スケールを0にして見えないように
        transform.localScale = Vector3.zero;

        //操作関連のイベントを登録
        TouchEventHandler.Instance.onBeginPress += OnBeginPress;
        TouchEventHandler.Instance.onEndPress += OnEndPress;
        TouchEventHandler.Instance.onEndDrag += OnEndDrag;
        TouchEventHandler.Instance.onDrag += OnDrag;
    }

    /// <summary>
    /// 必要なら初期化する
    /// </summary>
    public void InitIfNeeded()
    {
        //既に初期化済みならスルー
        if (IsInitialized)
        {
            return;
        }

        //スティック生成、位置を中心に設定
        _stick = new GameObject(STICK_NAME);
        _stick.transform.SetParent(gameObject.transform);
        _stick.transform.localRotation = Quaternion.identity;
        _stickPosition = Vector3.zero;

        //スティックにImageセット
        _stick.AddComponent<Image>();
    }

    //=================================================================================
    //計算
    //=================================================================================

    //タッチされていいる座標をワールド座標で取得
    private Vector3 GetTouchPointInWorld()
    {
        //タップされている位置を画面内の座標に変換
        Vector2 screenPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(),
          new Vector2(Input.mousePosition.x, Input.mousePosition.y),
          null,
          out screenPos
        );

        return screenPos;
    }

    //=================================================================================
    //タップ
    //=================================================================================

    //タップ開始時
    public void OnBeginPress()
    {
        //スケールを1にして見えるように
        transform.localScale = Vector3.one;

        //全体をタッチされている場所に移動
        transform.localPosition = Vector3.zero;
        transform.localPosition = GetTouchPointInWorld();

        //スティックの位置を中心に
        _stickPosition = Vector3.zero;
    }

    //タップ終了時(ドラッグ終了時には呼ばれない)
    public void OnEndPress()
    {
        //タップした終了した時にドラッグを終了した時と同じ処理をする
        OnEndDrag();
    }

    //=================================================================================
    //ドラッグ
    //=================================================================================

    //ドラッグ終了時
    public void OnEndDrag()
    {
        //スティックの位置を中心に
        _stickPosition = Vector3.zero;

        //スケールを0にして見えないように
        transform.localScale = Vector3.zero;
    }

    //ドラッグ中
    public void OnDrag(Vector2 delta)
    {
        //スティックをタップされている場所に移動
        _stickPosition = GetTouchPointInWorld();

        //移動場所が設定した半径を超えてる場合は制限内に抑える
        float currentRadius = Vector3.Distance(Vector3.zero, _stick.transform.localPosition);
        if (currentRadius > _radius)
        {

            //角度計算
            float radian = Mathf.Atan2(_stick.transform.localPosition.y, _stick.transform.localPosition.x);

            //円上にXとYを設定
            Vector3 limitedPosition = Vector3.zero;
            limitedPosition.x = _radius * Mathf.Cos(radian);
            limitedPosition.y = _radius * Mathf.Sin(radian);

            _stickPosition = limitedPosition;
        }

    }

    //=================================================================================
    //更新
    //=================================================================================

#if UNITY_EDITOR
    //Gizmoを表示する
    private void OnDrawGizmos()
    {
        //スティックが移動できる範囲をScene上に表示
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, _radius * 0.5f);
    }
#endif

}