using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CircleOutline : BaseMeshEffect
{
    [SerializeField]
    private Color m_EffectColor = new Color(0f, 0f, 0f, 0.5f);

    [SerializeField]
    private float m_EffectDistance = 1.0f;

    [SerializeField]
    private int m_nEffectNumber = 4;

    public Image target;

    //----------------------------------------------------------------------------------------------------------
    private Material material;
    private Image image;

    public Color EffectColor
    {
        set { m_EffectColor = value; }
        get { return m_EffectColor; }
    }

    protected override void Awake()
    {
        material = new Material(Shader.Find("GUI/Text Shader"));
        image = GetComponent<Image>();
        if (image != null) image.material = material;
        base.Awake();
    }

    private void Update()
    {
        if (image != null && image.sprite != target.sprite)
        {
            image.sprite = target.sprite;
        }
        material.SetColor("_Color", m_EffectColor);
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
            return;

        List<UIVertex> list = new List<UIVertex>();
        vh.GetUIVertexStream(list);

        ModifyVertices(list);

        vh.Clear();
        vh.AddUIVertexTriangleStream(list);
    }

    void ModifyVertices(List<UIVertex> verts)
    {
        int start = 0;
        int end = verts.Count;

        for (int n = 0; n < m_nEffectNumber; ++n)
        {
            float rad = 2.0f * Mathf.PI * n / m_nEffectNumber;
            float x = m_EffectDistance * Mathf.Cos(rad);
            float y = m_EffectDistance * Mathf.Sin(rad);

            ApplyShadow(verts, start, end, x, y);

            start = end;
            end = verts.Count;
        }
    }

    void ApplyShadow(List<UIVertex> verts, int start, int end, float x, float y)
    {
        UIVertex vt;

        for (int i = start; i < end; ++i)
        {
            vt = verts[i];
            verts.Add(vt);

            Vector3 v = vt.position;
            v.x += x;
            v.y += y;
            vt.position = v;
            Color32 newColor = m_EffectColor;
            vt.color = newColor;
            verts[i] = vt;
        }
    }
}
