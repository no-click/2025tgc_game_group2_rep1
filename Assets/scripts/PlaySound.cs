using UnityEngine;

public class PlaySound : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // 既存のAudioSourceコンポーネントを取得
        audioSource = GetComponent<AudioSource>();

        // AudioSourceコンポーネントがなければ追加する
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySE(AudioClip clip)
    {
        // clipが設定されていれば再生する
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        // clipが設定されていればBGMとして再生する
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.loop = true; // BGMとしてループ再生
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