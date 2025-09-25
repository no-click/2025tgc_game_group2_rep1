using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    // 静的なインスタンス変数
    public static SoundPlayer instance;
    private AudioSource audioSource;

    void Awake()
    {
        // インスタンスがまだ存在しない場合、このインスタンスを割り当てる
        if (instance == null)
        {
            instance = this;
            // シーンをまたいでオブジェクトを維持したい場合
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            // すでにインスタンスが存在する場合、このオブジェクトを破棄する
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