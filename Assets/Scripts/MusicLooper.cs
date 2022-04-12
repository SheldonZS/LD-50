using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLooper : MonoBehaviour
{
    public AudioSource bgm;

    public bool loop;
    public float loopStart;
    public float loopEnd;


    void Awake()
    {
        bgm = GameObject.Find("BGM").GetComponent<AudioSource>();

    }

    private void Start()
    {
       PlayMenuLoop();
        Debug.Log(bgm.clip.ToString());
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (loop & bgm.time >= loopEnd)
            bgm.time += loopStart - loopEnd;
    }

    public void PlayMenuLoop()
    {
        AudioClip song = Resources.Load<AudioClip>("BGM/#50_MenuLoop");
        PlayCustomLoop(song, 19.944f, 0);
    }

    public IEnumerator PlayMenuLoopAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayMenuLoop();
    }

    public void PlayCustomLoop(AudioClip song, float start = 0, float end = 0)
    {
        bgm.clip = song;
        bgm.loop = true;
        bgm.time = 0;
        bgm.Play();

        if (end <= start)
            end = song.length;

        loop = true;
        loopStart = start;
        loopEnd = end;
    }

    public void NewCustomLoop(AudioClip song, float start = 0, float end = 0, float fadeDuration = .5f, float volume = 1)
    {
        if (song == bgm.clip)
            return;

        StartCoroutine(NewLoopCR(song, start, end, fadeDuration, volume));
    }

    public IEnumerator NewLoopCR(AudioClip song, float start = 0, float end = 0, float fadeDuration = .5f, float volume = 1)
    {
        if (bgm.isPlaying)
            yield return FadeOut(fadeDuration, volume);

        bgm.volume = volume;
        PlayCustomLoop(song, start, end);
    }

    public void PlayOneShot(AudioClip song, float volume = 1)
    {
        bgm.clip = song;
        bgm.loop = false;
        bgm.time = 0;
        bgm.volume = volume;
        bgm.Play();

        loop = false;
    }

    public void FadeAndPlay(AudioClip song, float fadeDuration = 0.5f, float volume = 1)
    {
        StartCoroutine(FadePlayCR(song, fadeDuration, volume));
    }

    public IEnumerator FadePlayCR(AudioClip song, float fadeDuration = 0.5f, float volume = 1)
    {
        if (bgm.isPlaying)
            yield return FadeOut(fadeDuration, volume);
        PlayOneShot(song, volume);
    }

    public IEnumerator FadeOut(float fadeDuration = 0.5f, float volume = 1)
    {
        float startTime = Time.time;

        while (Time.time < startTime + fadeDuration)
        {
            bgm.volume = (1 - (Time.time - startTime) / fadeDuration) * volume;
            yield return null;
        }

        bgm.Pause();
    }

    public IEnumerator FadeIn(float fadeDuration = 0.5f, float volume = 1)
    {
        float startTime = Time.time;

        while (Time.time < startTime + fadeDuration)
        {
            bgm.volume = ((Time.time - startTime) / fadeDuration) * volume;
            yield return null;
        }

    }

    public void InsertLick(AudioClip song, float fadeDuration = 0.5f, float volume = 1)
    {
        StartCoroutine(InsertCR(song, fadeDuration, volume));
    }

    public IEnumerator InsertCR(AudioClip song, float fadeDuration, float volume)
    {
        yield return FadeOut(fadeDuration, volume);

        AudioClip original = bgm.clip;
        bool originalLooped = loop;
        float bgmTime = bgm.time;

        bgm.clip = song;
        bgm.time = 0;
        bgm.loop = false;
        loop = false;
        bgm.volume = volume;
        bgm.Play();

        while (bgm.isPlaying) yield return null;

        bgm.clip = original;
        bgm.loop = originalLooped;
        loop = originalLooped;
        bgm.Play();
        bgm.time = bgmTime;

        yield return FadeIn(fadeDuration, volume);
    }

    public IEnumerator StartFade(float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = bgm.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            bgm.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);

           // Debug.Log(bgm.volume + " " + currentTime + " " + duration);
            yield return null;
        }
        bgm.volume = targetVolume;
        yield break;
    }
}
