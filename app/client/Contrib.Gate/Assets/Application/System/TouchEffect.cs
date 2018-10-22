using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;
using Effect;

public class TouchEffect : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    public static void RuntimeInitializeOnLoadMethod()
    {
        var go = new GameObject("TouchEffect");
        go.AddComponent<TouchEffect>();
        GameObject.DontDestroyOnLoad(go);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            PlayTouchEffect(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            //EffectManager.Instance.Play("Simple_Ring_Shape2", Vector3.zero);
        }
        if (Input.touchCount > 0)
        {
            PlayTouchEffect(Input.touches[0].position);
        }
    }

    private void PlayTouchEffect(Vector3 screenPos)
    {
        EffectWindow.Instance.Play("Simple_GeneratingPosition1", screenPos);
    }
}
