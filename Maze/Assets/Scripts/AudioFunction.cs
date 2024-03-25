using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// ボタンやアニメーションなどで音を鳴らす関数をまとめたクラスです。
/// </summary>
public class AudioFunction : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    [Header("二種類の音を出す関数の設定")]
    [SerializeField] int more;
    [SerializeField] string clipIndx1;
    [SerializeField] string clipIndx2;
    public void PlaySE_2Type()
    {
        var f = animator.GetFloat("CompassRotation");

        string clipIndx = f > more ? clipIndx1 : clipIndx2;
        AudioManager.PlayOneShot(clipIndx);
    }



}
