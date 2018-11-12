using UnityEngine;

public class BillBoard : MonoBehaviour
{
    /// <summary>
    /// 縦方向への回転を制御します。
    /// </summary>
    public bool enableVerticalRotation = true;

    void Update()
    {
        Vector3 p = Camera.main.transform.position;
        if(!enableVerticalRotation) p.y = transform.position.y;
        transform.LookAt(p);
    }
}
