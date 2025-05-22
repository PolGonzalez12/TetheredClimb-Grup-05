using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager I;            

    [Header("Clips")]
    public AudioClip mainTheme;
    public AudioClip coinCollect;
    public AudioClip jumpSound;
    public AudioClip speakingSound;

    [Header("Volúmenes")]
    [Range(0, 1)] public float bgmDefaultVol = 0.05f; 
    [Range(0, 1)] public float bgmDuckedVol = 0.02f; 
    public float duckFadeTime = 0.05f;

    AudioSource bgmSource;   
    AudioSource sfxSource;   
    AudioSource voiceSource; 
    Coroutine duckRoutine;

    void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;

        sfxSource = gameObject.AddComponent<AudioSource>();

        voiceSource = gameObject.AddComponent<AudioSource>(); 
        voiceSource.loop = false;
    }
    public void PlayVoice(AudioClip clip, float vol = 1f, bool loop = false)
    {
        if (!clip) return;
        voiceSource.clip = clip;
        voiceSource.volume = vol;
        voiceSource.loop = loop;
        voiceSource.Play();
    }

    public void StopVoice() => voiceSource.Stop();

    public void PlayMainTheme()
    {
        PlayBGM(mainTheme, bgmDefaultVol);
    }

    public void PlayBGM(AudioClip clip, float vol)
    {
        if (!clip) return;
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;

        bgmSource.clip = clip;
        bgmSource.volume = vol;
        bgmSource.Play();
    }

    public void StopBGM() => bgmSource.Stop();

    public void DuckBGM() => StartDucking(bgmDuckedVol);
    public void UnduckBGM() => StartDucking(bgmDefaultVol);

    void StartDucking(float targetVol)
    {
        if (duckRoutine != null) StopCoroutine(duckRoutine);
        duckRoutine = StartCoroutine(FadeVolume(targetVol, duckFadeTime));
    }

    System.Collections.IEnumerator FadeVolume(float target, float time)
    {
        float start = bgmSource.volume;
        float t = 0f;
        while (t < time)
        {
            t += Time.unscaledDeltaTime;
            bgmSource.volume = Mathf.Lerp(start, target, t / time);
            yield return null;
        }
        bgmSource.volume = target;
    }

    public void PlaySFX(AudioClip clip, float vol = 1f)
    {
        if (clip) sfxSource.PlayOneShot(clip, vol);
    }
}
