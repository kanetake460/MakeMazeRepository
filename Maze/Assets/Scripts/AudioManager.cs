using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    [Header("クリップ")]
    [SerializeField] 
    AudioClip[] Clips_SE;
    [SerializeField] 
    AudioClip[] Clips_BGM;

    [Header("音量")]
    [SerializeField,Range(0f, 1f)]
    float Volume_SE;
    [SerializeField, Range(0f, 1f)]
    float Volume_BGM;

    static AudioClip[] _se;
    static AudioClip _selectedSE;
    static AudioClip[] _bgm;
    static AudioClip _selectedBGM;

    static int _seCount;

    static AudioSource _BGMAudioSource;
    static AudioSource _SEAudioSource;

    

    private void Awake()
    {
        if (_BGMAudioSource == null)
            _BGMAudioSource = gameObject.AddComponent<AudioSource>();
        if (_SEAudioSource  == null)
            _SEAudioSource  = gameObject.AddComponent<AudioSource>();

    }

    private void Start()
    {
        _se = Clips_SE;
        _bgm = Clips_BGM;
    }

    private void Update()
    {
        SetVolumeSE(Volume_SE);
        SetVolumeBGM(Volume_SE);

        PlaySEContinue();
        if (Input.GetKeyDown(KeyCode.P))
            PlaySEStart(0,5);
    }

    /// <summary>
    /// SEの音量を設定します
    /// </summary>
    /// <param name="volume">音量（0～1）</param>
    static void SetVolumeSE(float volume)
    {
        _BGMAudioSource.volume = volume;
    }

    /// <summary>
    /// BGMの音量を設定します
    /// </summary>
    /// <param name="volume">音量（0～1）</param>
    static void SetVolumeBGM(float volume)
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

    static void PlaySEStart(int clipIndx,int count = 1)
    {
        _seCount = count;
        _selectedSE = _se[clipIndx];
    }


    public void PlayBGM(int clipIndx)
    {
        _SEAudioSource.clip = Clips_BGM[clipIndx];
        _SEAudioSource.UnPause();
    }

    public void RePlayBGM()
    {

    }

    public void StopBGM()
    {
        _SEAudioSource.Pause();
    }
}
