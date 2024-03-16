using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// オーディオソースのディクショナリ
/// </summary>
[Serializable]
public class AudioSourceDictionary
{
    public string key;
    public AudioSource value;
}

/// <summary>
/// オーディオクリップのディクショナリ
/// </summary>
[Serializable]
public class AudioClipDictionary
{
    public string key;
    public AudioClip value;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("オーディオソース")]
    [SerializeField]
    List<AudioSourceDictionary> audioSourceDictionary = new List<AudioSourceDictionary>();
    static Dictionary<string, AudioSource> audioSourceDic = new Dictionary<string, AudioSource>();

    [Header("クリップ")]
    [SerializeField] List<AudioClipDictionary> SE_ClipDictionary = new List<AudioClipDictionary>();
    static Dictionary<string,AudioClip> _DicSE = new Dictionary<string, AudioClip>();
    
    [SerializeField] List<AudioClipDictionary> BGM_ClipDictionary = new List<AudioClipDictionary>();
    static Dictionary<string, AudioClip> _DicBGM = new Dictionary<string, AudioClip>();


    [Header("音量")]
    [SerializeField,Range(0f, 1f)]
    float Volume_SE = 0.5f;
    [SerializeField, Range(0f, 1f)]
    float Volume_BGM = 0.5f;

    static AudioClip _selectedSE;
    static AudioClip _selectedBGM;

    static int _seCount;

    static AudioSource _BGMAudioSource;
    static AudioSource _SEAudioSource;

    static Animator anim;

    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        anim = GetComponent<Animator>();
        if (_BGMAudioSource == null)
            _BGMAudioSource = gameObject.AddComponent<AudioSource>();
        if (_SEAudioSource  == null)
            _SEAudioSource  = gameObject.AddComponent<AudioSource>();

    }

    private void Start()
    {
        foreach(var source in audioSourceDictionary)
        {
            audioSourceDic[source.key] = source.value;
        }
        foreach(var clip in SE_ClipDictionary)
        {
            _DicSE[clip.key] = clip.value;
        }
        foreach (var clip in BGM_ClipDictionary)
        {
            _DicBGM[clip.key] = clip.value;
        }
    }

    private void Update()
    {
        SetVolumeSE(Volume_SE);
        SetVolumeBGM(Volume_SE);

        PlaySEContinue();

    }

    /// <summary>
    /// SEの音量を設定します
    /// </summary>
    /// <param name="volume">音量（0～1）</param>
    public static void SetVolumeSE(float volume)
    {
        _BGMAudioSource.volume = volume;
    }

    /// <summary>
    /// BGMの音量を設定します
    /// </summary>
    /// <param name="volume">音量（0～1）</param>
    public static void SetVolumeBGM(float volume)
    {
        _SEAudioSource.volume = volume;
    }

    /// <summary>
    /// SEを鳴らし続けます
    /// </summary>
    /// <param name="clipIndx"></param>
    private void PlaySEContinue()
    {
        if(_SEAudioSource.isPlaying == false && _seCount > 0)
        {
            _seCount--;
            _SEAudioSource.PlayOneShot(_selectedSE);
        }
    }

    public static void PlaySEStart(string clipKey,int count = 1)
    {
        _seCount = count;
        _selectedSE = _DicSE[clipKey];
    }


    public static void PlayOneShot(string clipKey)
    {
        Debug.Log(_DicSE.Count);
        _SEAudioSource.PlayOneShot(_DicSE[clipKey]);
    }
    public static void PlayOneShot(string clipKey,AudioSource audioSource)
    {
        audioSource.PlayOneShot(_DicSE[clipKey]);
    }


    public static void PlayBGM(string clipKey)
    {
        if(_BGMAudioSource.isPlaying == false)
        {
            _BGMAudioSource.clip = _DicBGM[clipKey];
            _BGMAudioSource.Play();
        }
    }
    public static void PlayBGM(string clipKey,AudioSource audioSource)
    {
        if (audioSource.isPlaying == false)
        {
            audioSource.clip = _DicBGM[clipKey];
            audioSource.Play();
            Debug.Log(audioSource.isPlaying);
        }
    }

    public static void RePlayBGM()
    {
        _BGMAudioSource.UnPause();
    }

    public static void StopBGM() { _BGMAudioSource.Pause(); }
    public static void StopBGM(AudioSource audioSource) { audioSource.Pause(); }
        


}
