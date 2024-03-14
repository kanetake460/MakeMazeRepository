using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioFunction : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    [Header("“ñŽí—Þ‚Ì‰¹‚ðo‚·ŠÖ”‚ÌÝ’è")]
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
