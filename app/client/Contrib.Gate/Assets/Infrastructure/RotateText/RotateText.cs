using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Text))]
public class RotateText : UIBehaviour, IMeshModifier
{
    private Text textComponent;
    private char[] characters;

    // 回転させない文字群
    // XXX 別の設定ファイルなりcsvにまとめて最初に読み込んでしまうのが良さそう
    public List<char> nonrotatableCharacters;

#if UNITY_EDITOR
    public new void OnValidate()
    {
        base.OnValidate();
        textComponent = this.GetComponent<Text>();

        var graphics = base.GetComponent<Graphic>();
        if (graphics != null)
        {
            graphics.SetVerticesDirty();
        }
    }
#endif

    public void ModifyMesh(Mesh mesh) { }
    public void ModifyMesh(VertexHelper verts)
    {
        if (!this.IsActive())
        {
            return;
        }

        List<UIVertex> vertexList = new List<UIVertex>();
        verts.GetUIVertexStream(vertexList);

        ModifyVertices(vertexList);

        verts.Clear();
        verts.AddUIVertexTriangleStream(vertexList);
    }

    void ModifyVertices(List<UIVertex> vertexList)
    {
        characters = textComponent.text.ToCharArray();
        if (characters.Length == 0)
        {
            return;
        }

        for (int i = 0, vertexListCount = vertexList.Count; i < vertexListCount; i += 6)
        {
            int index = i / 6;
            if (IsNonrotatableCharactor(characters[index]))
            {
                continue;
            }

            var center = Vector2.Lerp(vertexList[i].position, vertexList[i + 3].position, 0.5f);
            for (int r = 0; r < 6; r++)
            {
                var element = vertexList[i + r];
                var pos = element.position - (Vector3)center;
                var newPos = new Vector2(
                    pos.x * Mathf.Cos(90 * Mathf.Deg2Rad) - pos.y * Mathf.Sin(90 * Mathf.Deg2Rad),
                    pos.x * Mathf.Sin(90 * Mathf.Deg2Rad) + pos.y * Mathf.Cos(90 * Mathf.Deg2Rad)
                );

                element.position = (Vector3)(newPos + center);
                vertexList[i + r] = element;
            }
        }
    }

    bool IsNonrotatableCharactor(char character)
    {
        if (nonrotatableCharacters == null) return false;
        return nonrotatableCharacters.Any(x => x == character);
    }
}