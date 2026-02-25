using System;
using UnityEngine;
using System.Collections.Generic;

// 배경음악(BGM)
public enum Enum_Bgm
{
    TITLE,
    GAME_BACKGOUND,
    RESULT,
}

// 효과음(SFX)
public enum Enum_Sfx
{
    FALL1,
    FALL2,
    PLACE_STONE1,
    PLACE_STONE2,
    PLACE_STONE3,
    BLOCKED_STONE,
    WINNING1,
    WINNING2,
    WINNING3,
}

public class SoundManager : MonoBehaviour
{
public static SoundManager instance;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] bgmClips;  // BGM 클립 배열
    [SerializeField] private AudioClip[] sfxClips; // SFX 클립 배열

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource; // BGM 재생 AudioSource
    [SerializeField] private AudioSource sfxSource; // SFX 재생 AudioSource

    private Dictionary<Enum_Bgm, AudioClip> bgmDict; // BGM Dictionary
    private Dictionary<Enum_Sfx, AudioClip> sfxDict; // SFX Dictionary

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitDictionaries();
    }

    // Dictionary 초기화
    private void InitDictionaries()
    {
        bgmDict = new Dictionary<Enum_Bgm, AudioClip>();
        for (int i = 0; i < bgmClips.Length; i++)
        {
            bgmDict[(Enum_Bgm)i] = bgmClips[i];
        }

        sfxDict = new Dictionary<Enum_Sfx, AudioClip>();
        for (int i = 0; i < sfxClips.Length; i++)
        {
            sfxDict[(Enum_Sfx)i] = sfxClips[i];
        }
    }

    // BGM 재생
    public void PlayBGM(Enum_Bgm bgmType)
    {
        if (bgmDict.TryGetValue(bgmType, out var clip))
        {
            bgmSource.clip = clip;
            bgmSource.loop = true; // 배경음악은 기본적으로 반복 재생
            bgmSource.volume = 0.2f;    // 기본 20% 볼륨
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("BGM not found in Dictionary!");
        }
    }

    // SFX 재생
    public void PlaySFX(Enum_Sfx sfxType)
    {
        if (sfxDict.TryGetValue(sfxType, out var clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SFX not found in Dictionary!");
        }
    }
}