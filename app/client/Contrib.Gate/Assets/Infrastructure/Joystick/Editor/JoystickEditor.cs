//  JoystickEditor.cs
//  http://kan-kikuchi.hatenablog.com/entry/uGUI_Joystick_2
//
//  Created by kan.kikuchi on 2016.07.19.

using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// JoystickのInspectorを変えるエディター
/// </summary>
[CustomEditor(typeof(Joystick))]
public class JoystickEditor : Editor
{

    //プロパティーを編集するためのクラス
    private SerializedProperty _radiusProperty;
    private SerializedProperty _positionProperty;

    //=================================================================================
    //初期化
    //=================================================================================

    private void OnEnable()
    {
        _radiusProperty = serializedObject.FindProperty("_radius");
        _positionProperty = serializedObject.FindProperty("_position");
    }

    //=================================================================================
    //更新
    //=================================================================================

    //Inspectorを更新する
    public override void OnInspectorGUI()
    {

        //更新開始
        serializedObject.Update();

        //ジョイスティックを取得
        Joystick joystick = target as Joystick;

        //初期化していない時は初期化ボタンを表示
        if (!joystick.IsInitialized)
        {
            if (GUILayout.Button("Init"))
            {
                joystick.InitIfNeeded();
            }
        }
        else
        {
            //スティックが動く範囲の半径
            float radius = EditorGUILayout.FloatField("動作範囲の半径", _radiusProperty.floatValue);
            if (radius != _radiusProperty.floatValue)
            {
                _radiusProperty.floatValue = radius;
            }

            //現在地を表示
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField(
              "現在地(-1~1)   X : " +
              _positionProperty.vector2Value.x.ToString("F2") + ",  Y : " +
              _positionProperty.vector2Value.y.ToString("F2")
            );
            EditorGUILayout.EndVertical();
        }

        //更新終わり
        serializedObject.ApplyModifiedProperties();
    }

}