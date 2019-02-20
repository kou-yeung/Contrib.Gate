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
        if (Input.GetMouseButtonDown(0))
        {
            PlayTouchEffect(Input.mousePosition);
        }
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            PlayTouchEffect(Input.touches[0].position);
        }
    }

    private void PlayTouchEffect(Vector3 screenPos)
    {
        EffectWindow.Instance.Play("回復10", screenPos);
    }
}
