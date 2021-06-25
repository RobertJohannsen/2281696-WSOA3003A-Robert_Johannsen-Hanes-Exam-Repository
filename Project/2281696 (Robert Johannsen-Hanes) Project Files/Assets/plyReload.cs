using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plyReload : MonoBehaviour
{
    public GameObject reloadBarBase, reloadPointer , fancyPointer, magPrefab,magPort;
    public enum reloadState { reloadBase, reloadEject , reloadInsert};
    public reloadState state;
    public float reloadTime, insertTimeStart, insertTimeEnd , reloadStep ; //reloadtime is max reload time without fancy reload in frames;
    public float istPerc, istePerc,baseXScale;
    public int elapsedFrames,insertStartFrame , insertEndFrame ;
    public bool showReloadUI;
    public int magSize , ammoPool;
    public static plyReload reCont;
    // Start is called before the first frame update

    void Awake()
    {
        reCont = this;
    }
    void Start()
    {
        baseXScale = reloadBarBase.transform.localScale.x;
        reloadPointer.transform.localPosition = new Vector3(-0.5f, 0, -.5f);
        
    }

    // Update is called once per frame
    void Update()
    {

        if(EnemyAi.enAI.finalHP != 0)
        {
            reloadStep = elapsedFrames / reloadTime;
            istPerc = (insertTimeStart / 100);
            if ((insertTimeEnd + insertTimeStart) > 100)
            {
                istePerc = 1;
                fancyPointer.transform.localScale = new Vector3(((100 - insertTimeStart) / 100), fancyPointer.transform.localScale.y, 1);
                fancyPointer.transform.localScale = new Vector3(((100 - insertTimeStart) / 100), fancyPointer.transform.localScale.y, 1);

            }
            else
            {
                istePerc = ((insertTimeEnd + insertTimeStart) / 100);
                fancyPointer.transform.localScale = new Vector3((insertTimeEnd / 100), fancyPointer.transform.localScale.y, 1);
            }

            insertStartFrame = (int)(istPerc * reloadTime);
            insertEndFrame = (int)(istePerc * reloadTime);
            fancyPointer.transform.localPosition = new Vector3(-0.5f + istPerc + ((insertTimeEnd / 100) / 2), fancyPointer.transform.localPosition.y, fancyPointer.transform.localPosition.z);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (ammoPool > 0)
                {
                    switch (state)
                    {
                        case reloadState.reloadBase:
                            dropMag();
                            state = reloadState.reloadEject;
                            break;

                    }
                }

            }

            if (state == reloadState.reloadEject)
            {
                showReloadUI = true;
            }
            else
            {
                showReloadUI = false;
            }

            reloadUI();
            handleReload();
        }
        else
        {
            Destroy(reloadBarBase);
            Destroy(reloadPointer);
            Destroy(fancyPointer);
        }
      
    }

    void reloadUI()
    {
        reloadBarBase.GetComponent<SpriteRenderer>().enabled = showReloadUI;
        fancyPointer.GetComponent<SpriteRenderer>().enabled = showReloadUI;
        reloadPointer.GetComponent<SpriteRenderer>().enabled = showReloadUI;
    }

    void handleReload()
    {
        switch (state)
        {
            case reloadState.reloadEject:
                elapsedFrames++;

                reloadPointer.transform.localPosition = new Vector3(-0.5f+(elapsedFrames/reloadTime),reloadPointer.transform.localPosition.y , reloadPointer.transform.localPosition.z);

                if((elapsedFrames >= insertStartFrame) && (elapsedFrames <= insertEndFrame))
                {
                    if(Input.GetKeyDown(KeyCode.Space))
                    {
                        elapsedFrames = 0;
                       
                        state = reloadState.reloadInsert;
                    }
                }

                if(elapsedFrames == reloadTime)
                {
                    elapsedFrames = 0;
                    state = reloadState.reloadInsert;
                }
            
                break;
            case reloadState.reloadInsert:
                doReload();
                state = reloadState.reloadBase;
                break;
        }
    }

    void dropMag()
    {
        switch (plyCont.plycont.currentWeapon.cycleType)
        {
            case WeaponController.cycleType.auto:
                Instantiate(magPrefab, magPort.transform.position, Quaternion.identity);
                break;
            case WeaponController.cycleType.revolver:
                switch (plyCont.plycont.weptype)
                {
                    case 1:
                        bulletController.bCont.SpawnProjectiles(plyCont.plycont.currentWeapon.magSize, 100, 0.1f, plyCont.plycont.ejectionPort.transform.position, Random.Range(-5, 5), 20, true, plyCont.plycont.pistCasing);
                        break;

                    case 2:
                        bulletController.bCont.SpawnProjectiles(plyCont.plycont.currentWeapon.magSize, 100, 0.1f, plyCont.plycont.ejectionPort.transform.position, Random.Range(-5, 5), 20, true, plyCont.plycont.shotCasing);
                        break;
                    case 4:
                        bulletController.bCont.SpawnProjectiles(plyCont.plycont.currentWeapon.magSize, 100, 0.1f, plyCont.plycont.ejectionPort.transform.position, Random.Range(-5, 5), 20, true, plyCont.plycont.rockCasing);
                        break;
                    case 5:
                        bulletController.bCont.SpawnProjectiles(plyCont.plycont.currentWeapon.magSize, 100, 0.1f, plyCont.plycont.ejectionPort.transform.position, Random.Range(-5, 5), 20, true, plyCont.plycont.rifleCasing);
                        break;


                }
              
                break;
        }
      
    }

    void doReload()
    {
        if((ammoPool-magSize) >= 0)
        {
            plyCont.plycont.currentAmmo = magSize;
            ammoPool -= magSize;
        }
        else
        {
            if(ammoPool != 0)
            {
                plyCont.plycont.currentAmmo = ammoPool;
                ammoPool = 0;
            }
        }
        

        //do reload stuff
    }
}
