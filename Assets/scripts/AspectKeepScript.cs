using UnityEngine;

public class AspectKeepScript : MonoBehaviour
{
    //�ڕW�̔䗦
    [SerializeField] private Vector2 _aspectVec = new Vector2(16, 9);
    //�J����
    private Camera _camera;
    // Start is called before the first frame update
    void Start()
    {
        //�����̃J����
        _camera = GetComponent<Camera>();
        //��ʔ�ϊ�
        AspectKeep();
    }
    // Update is called once per frame
    void Update()
    {
        //��ʔ�ϊ�
        AspectKeep();
    }
    //��ʔ�ϊ�
    void AspectKeep()
    {
        //��ʂ̃A�X�y�N�g��ƃJ�����̃A�X�y�N�g��
        float screenAspect = (float)Screen.width / Screen.height;
        float targetAspect = _aspectVec.x / _aspectVec.y;
        //�{��
        float magRate = targetAspect / screenAspect;
        //ViewportRect
        Rect viewportRect = new Rect(0, 0, 1, 1);
        if (magRate < 1)
        {
            //�����Ȃ牡���̕ύX
            viewportRect.width = magRate;
            //������
            viewportRect.x = 0.5f - viewportRect.width * 0.5f;
        }
        else
        {
            //�c���Ȃ�c����ύX
            viewportRect.height = 1 / magRate;
            //������
            viewportRect.y = 0.5f - viewportRect.height * 0.5f;
        }
        //�J������Viewport�ɑ��
        _camera.rect = viewportRect;
    }
}