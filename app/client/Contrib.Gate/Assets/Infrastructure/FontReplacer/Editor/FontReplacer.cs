using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

///--------------------------------------------- 
/// <summary> 
/// フォント一括入れ替え 
/// </summary> 
///--------------------------------------------- 
public class FontReplacer : EditorWindow
{
    string[] sarchDir = { "Assets/Resources" };
    private Font fontData;

    [MenuItem("Tools/Font/FontReplecer")]
    public static void GetWindow()
    {
        EditorWindow.GetWindow(typeof(FontReplacer)).Show();
    }

    ///--------------------------------------------- 
    /// <summary> 
    /// 更新 
    /// </summary> 
    ///--------------------------------------------- 
    void OnGUI()
    {
        this.fontData = EditorGUILayout.ObjectField("Font", this.fontData, typeof(Font), true) as Font;
        if (this.fontData == null)
        {
            return;
        }

        if (GUILayout.Button("Replace font in all assets"))
        {
            Replace(this.fontData);
        }
    }

    ///--------------------------------------------- 
    /// <summary> 
    /// 更新 
    /// </summary> 
    ///--------------------------------------------- 
    private void Replace(Font _fontData)
    {
        string title = "Replacing [" + _fontData.name + "]";
        string[] guids = AssetDatabase.FindAssets("", sarchDir);
        //string[] guids = AssetDatabase.FindAssets ("l:concrete"); 

        bool isSave = false;
        for (int ii = 0; ii < guids.Length; ii++)
        {
            string guid = guids[ii];
            string guidPath = AssetDatabase.GUIDToAssetPath(guid);
            EditorUtility.DisplayProgressBar(title, guidPath, (float)ii / (float)guids.Length);

            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(guidPath);
            if (go != null)
            {
                List<Text> textList = GetComponentsInParentAndChildren<Text>(go);
                for (int jj = 0; jj < textList.Count; jj++)
                {
                    Text textData = textList[jj];
                    if (textData != null)
                    {
                        textData.font = _fontData;
                        EditorUtility.SetDirty(textData);
                    }
                }
                isSave = true;
            }
        }
        if (isSave)
        {
            AssetDatabase.SaveAssets();
        }
        EditorUtility.ClearProgressBar();
    }

    private static List<T> GetComponentsInParentAndChildren<T>(GameObject target) where T : UnityEngine.Component
    {
        bool includeInactive = true;
        List<T> _list = new List<T>(target.GetComponents<T>());

        _list.AddRange(new List<T>(target.GetComponentsInChildren<T>(includeInactive)));
        _list.AddRange(new List<T>(target.GetComponentsInParent<T>(includeInactive)));

        return _list;
    }
}