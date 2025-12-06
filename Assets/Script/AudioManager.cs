using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    [Header("Audio Sources")]
    public AudioSource musicSource;

    public AudioSource sfxSource;

    [Header("Audio Clips (Kéo file âm thanh vào ?ây)")]
    public AudioClip backgroundMusic;
    public AudioClip clickSound;
    public AudioClip pourSound;
    public AudioClip winSound;
    //public AudioClip errorSound;


    private void Start()
    {
        PlayMusic(backgroundMusic);
    }
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayClick() => PlaySFX(clickSound);
    public void PlayPour() => PlaySFX(pourSound);
    public void PlayWin() => PlaySFX(winSound);
    //public void PlayError() => PlaySFX(errorSound);
}
