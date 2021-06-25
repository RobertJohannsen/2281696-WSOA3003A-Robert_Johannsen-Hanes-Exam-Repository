using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoHUDCont : MonoBehaviour
{
    public TextMeshProUGUI hudAmmoCount,plyHPhud ,plyroundHUD ;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(EnemyAi.enAI.finalHP != 0)
        {
            hudAmmoCount.text = plyReload.reCont.ammoPool + "";
            plyHPhud.text = plyCont.plycont.plyHP + "";
            plyroundHUD.text = plyCont.plycont.plyRoundCounter + "";
        } 
        else
        {
            hudAmmoCount.transform.position = new Vector3(1111, 11111, 1111);
            plyHPhud.transform.position = new Vector3(1111, 11111, 1111);
            plyroundHUD.transform.position = new Vector3(1111, 11111, 1111);
        }
     
    }
}
