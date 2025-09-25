using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    // �ÓI�ȃC���X�^���X�ϐ�
    public static SoundPlayer instance;
    private AudioSource audioSource;

    void Awake()
    {
        // �C���X�^���X���܂����݂��Ȃ��ꍇ�A���̃C���X�^���X�����蓖�Ă�
        if (instance == null)
        {
            instance = this;
            // �V�[�����܂����ŃI�u�W�F�N�g���ێ��������ꍇ
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            // ���łɃC���X�^���X�����݂���ꍇ�A���̃I�u�W�F�N�g��j������
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySE(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}