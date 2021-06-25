using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AmmoUi : MonoBehaviour
{
    public GameObject AmmoBarBase, ammoPointer;
    
    public float baseXScale;
    public float _step;
   
    public bool showAmmoUI;
  

    // Start is called before the first frame update

  
    void Start()
    {
        baseXScale = AmmoBarBase.transform.localScale.x;
      

    }

    // Update is called once per frame
    void Update()
    {
        if(EnemyAi.enAI.finalHP == 0)
        {
            Destroy(AmmoBarBase);
            Destroy(ammoPointer);
        } 
        else
        {
            int magSize = plyReload.reCont.magSize;
            float ammo = plyCont.plycont.currentAmmo;

            ammoPointer.transform.localPosition = new Vector3(-0.5f + (ammo / magSize), ammoPointer.transform.localPosition.y, ammoPointer.transform.localPosition.z);
            if (plyReload.reCont.state == plyReload.reloadState.reloadBase)
            {
                showAmmoUI = true;
            }
            else
            {
                showAmmoUI = false;
            }
            ammoUI();
        }
       
    }

    void ammoUI()
    {
        AmmoBarBase.GetComponent<SpriteRenderer>().enabled = showAmmoUI;
        ammoPointer.GetComponent<SpriteRenderer>().enabled = showAmmoUI;
       
    }

  
     
           
             

               
  

  
   

}
