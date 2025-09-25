using UnityEngine;

public class PlaySound : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // ������AudioSource�R���|�[�l���g���擾
        audioSource = GetComponent<AudioSource>();

        // AudioSource�R���|�[�l���g���Ȃ���Βǉ�����
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySE(AudioClip clip)
    {
        // clip���ݒ肳��Ă���΍Đ�����
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        // clip���ݒ肳��Ă����BGM�Ƃ��čĐ�����
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.loop = true; // BGM�Ƃ��ă��[�v�Đ�
            audioSource.Play();
        }
    }

    public void StopBGM()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}