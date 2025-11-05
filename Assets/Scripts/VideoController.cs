using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Inspector でアサイン
    public GameObject videoScreen;  // 動画を表示するオブジェクト（Quadなど）
    public GameObject videoScreen1;  // 動画を表示するオブジェクト（Quadなど）

    void Start()
    {
        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = true;
        videoScreen.SetActive(false); // 最初は非表示
        videoScreen1.SetActive(false); // 最初は非表示
    }

    public void PlayVideo()
    {
        videoScreen.SetActive(true);
        videoScreen1.SetActive(true);
        videoPlayer.Play();
    }

    public void StopVideo()
    {
        videoPlayer.Stop();
        videoScreen.SetActive(false);
        videoScreen1.SetActive(false);
    }

    public void PauseVideo()
    {
        videoPlayer.Pause();
    }

    public void ResumeVideo()
    {
        videoPlayer.Play();
    }
}
