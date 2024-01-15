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
    [SerializeField] GameObject[] HamburgerUI;

    public TextMeshProUGUI flagCountText;


    void Start()
    {
    }

    public void CountText()
    {
        flagCountText.text = gameManager.flags + " / " + gameManager.clearFlagNum;
    }

    public void HamburgerManager()
    {
        for(int i = 0; i < gameManager.hamburgerNum; i++) 
        {
            HamburgerUI[i].SetActive(false);
        }
        for(int i = 0;i < gameManager.hamburgerCount; i++)
        {
            HamburgerUI[i].SetActive(true);
        }
    }



    void Update()
    {
        anim.SetFloat("CompassRotation",roomCompass.distance);
        CountText();
        HamburgerManager();
    }
}
