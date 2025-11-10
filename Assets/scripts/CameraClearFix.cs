using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraClearFix : MonoBehaviour
{
    public Color clearColor = Color.black;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    // カメラが描画を始める直前に呼ばれる
    void OnPreCull()
    {
        // AspectKeepScript で縮められた rect を一時的に退避
        Rect oldRect = cam.rect;

        // いったん画面全体を描画範囲に広げる
        cam.rect = new Rect(0f, 0f, 1f, 1f);

        // 画面全体を指定色でクリア
        GL.Clear(true, true, clearColor);

        // 元の rect（レターボックス状態）に戻す
        cam.rect = oldRect;
    }
}
