using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �I�[�f�B�I�\�[�X�̃f�B�N�V���i��
/// </summary>
[Serializable]
public class AudioSourceDictionary
{
    public string key;
    public AudioSource value;
}

/// <summary>
/// �I�[�f�B�I�N���b�v�̃f�B�N�V���i��
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

    [Header("�I�[�f�B�I�\�[�X")]
    [SerializeField]
    List<AudioSourceDictionary> audioSourceDictionary = new List<AudioSourceDictionary>();
    static Dictionary<string, AudioSource> audioSourceDic = new Dictionary<string, AudioSource>();

    [Header("�N���b�v")]
    [SerializeField] List<AudioClipDictionary> SE_ClipDictionary = new List<AudioClipDictionary>();
    static Dictionary<string,AudioClip> _DicSE = new Dictionary<string, AudioClip>();
    
    [SerializeField] List<AudioClipDictionary> BGM_ClipDictionary = new List<AudioClipDictionary>();
    static Dictionary<string, AudioClip> _DicBGM = new Dictionary<string, AudioClip>();


    [Header("����")]
    [SerializeField,Range(0f, 1f)]
    float volume_SE = 0.5f;
    [SerializeField, Range(0f, 1f)]
    float volume_BGM = 0.5f;

    static AudioClip _selectedSE;
    static AudioClip _selectedBGM;

    static int _seCount;
    static bool _isBGM;

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
        SetVolumeSE(volume_SE);
        SetVolumeBGM(volume_BGM);

        PlaySEContinue();
    }

    /// <summary>
    /// SE�̉��ʂ�ݒ肵�܂�
    /// </summary>
    /// <param name="volume">���ʁi0�`1�j</param>
    public static void SetVolumeSE(float volume)
    {
        _SEAudioSource.volume = volume;
    }

    /// <summary>
    /// BGM�̉��ʂ�ݒ肵�܂�
    /// </summary>
    /// <param name="volume">���ʁi0�`1�j</param>
    public static void SetVolumeBGM(float volume)
    {
        _BGMAudioSource.volume = volume;
    }

    /// <summary>
    /// SE��炵�����܂�
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

    public static void PlaySEStart(string clipKey,int count = 1,float volume = 0.5f)
    {
        SetVolumeSE(volume);
        _seCount = count;
        _selectedSE = _DicSE[clipKey];
    }


    public static void PlayOneShot(string clipKey, float volume = 0.5f)
    {
        SetVolumeSE(volume);
        _SEAudioSource.PlayOneShot(_DicSE[clipKey]);
    }
    public static void PlayOneShot(string clipKey,AudioSource audioSource, float volume = 0.5f)
    {
        SetVolumeSE(volume);
        audioSource.PlayOneShot(_DicSE[clipKey]);
    }


    private void PlayBGMContinue()
    {
        if(_isBGM)
        {
            _isBGM = false;
            PlayBGM(_selectedBGM);
        }
    }


    public static void PlayBGMStart(string clipKey)
    {
        _selectedBGM = _DicBGM[clipKey];
    }

    public static void PlayBGMExit(string clipKey)
    {
        _isBGM = true;
    }

    public static void PlayBGM(string clipKey, float volume = 0.5f)
    {
        if(_BGMAudioSource.isPlaying == false)
        {
            _BGMAudioSource.clip = _DicBGM[clipKey];
            _BGMAudioSource.Play();
        }
    }

    public static void PlayBGM(AudioClip clip)
    {
        if(_BGMAudioSource.isPlaying == false)
        {
            _BGMAudioSource.clip = clip;
            _BGMAudioSource.Play();
        }
    }
    public static void PlayBGM(string clipKey,AudioSource audioSource)
    {
        if (audioSource.isPlaying == false)
        {
            audioSource.clip = _DicBGM[clipKey];
            audioSource.Play();
        }
    }

    public static void RePlayBGM()
    {
        _BGMAudioSource.UnPause();
    }

    public static void StopBGM() { _BGMAudioSource.Pause(); }
    public static void StopBGM(AudioSource audioSource) { audioSource.Pause(); }
        


}
