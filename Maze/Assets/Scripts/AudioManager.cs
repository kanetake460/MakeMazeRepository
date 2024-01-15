using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip sensorLv1_SE;
    [SerializeField] AudioClip sensorLv2_SE;

    [SerializeField] AudioSource audioSourceUI;


    void Start()
    {
        
    }

    public void PlaySE_SensorLv1()
    {
        audioSourceUI.PlayOneShot(sensorLv1_SE);
    }

    public void PlaySE_SensorLv2()
    {
        audioSourceUI.PlayOneShot(sensorLv2_SE);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}