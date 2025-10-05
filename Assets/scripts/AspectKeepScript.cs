using UnityEngine;

public class AspectKeepScript : MonoBehaviour
{
    //目標の比率
    [SerializeField] private Vector2 _aspectVec = new Vector2(16, 9);
    //カメラ
    private Camera _camera;
    // Start is called before the first frame update
    void Start()
    {
        //自分のカメラ
        _camera = GetComponent<Camera>();
        //画面比変換
        AspectKeep();
    }
    // Update is called once per frame
    void Update()
    {
        //画面比変換
        AspectKeep();
    }
    //画面比変換
    void AspectKeep()
    {
        //画面のアスペクト比とカメラのアスペクト比
        float screenAspect = (float)Screen.width / Screen.height;
        float targetAspect = _aspectVec.x / _aspectVec.y;
        //倍率
        float magRate = targetAspect / screenAspect;
        //ViewportRect
        Rect viewportRect = new Rect(0, 0, 1, 1);
        if (magRate < 1)
        {
            //横長なら横幅の変更
            viewportRect.width = magRate;
            //中央寄せ
            viewportRect.x = 0.5f - viewportRect.width * 0.5f;
        }
        else
        {
            //縦長なら縦幅を変更
            viewportRect.height = 1 / magRate;
            //中央寄せ
            viewportRect.y = 0.5f - viewportRect.height * 0.5f;
        }
        //カメラのViewportに代入
        _camera.rect = viewportRect;
    }
}