using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /*オブジェクト参照*/
    public GameManager gameManager;
    [SerializeField] Animator anim;
    [SerializeField] Compass roomCompass;

    public Image CompussSensorLv1;
    public Image CompussSensorLv2;
    public TextMeshProUGUI flagCountText;


    void Start()
    {
    }

    public void CountText()
    {
        flagCountText.text = gameManager.flags + " / " + gameManager.clearFlagNum;
    }

    void Update()
    {
        anim.SetFloat("CompassRotation",roomCompass.distance);
        CountText();
    }
}
