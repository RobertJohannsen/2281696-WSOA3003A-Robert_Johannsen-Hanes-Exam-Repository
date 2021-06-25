using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.SceneManagement;

public class plyCont : MonoBehaviour
{
    public GameObject weapon,crosshair,wepBarrel,bulletPrefab,ply,weapSpr,casing,ejectionPort,ghostPly,hole,warpAnim,zapAnim,muzzleFlash;
    public float xVal, yVal , moveSpeed , bulletAngle;
    public Vector3 moveVect,mousePos ,dir;
    public Rigidbody rb;
    public static plyCont plycont;
    public bool triggerDown,shootReady,doRecoil,recoilUp,dodge,visGhost,invincible,plyDied,finalDeath;
    public float maxRecoilAngle, currentRecoilDev, upRecoilStep, downRecoilStep ,baseAngle,recoilUpFrame,recoilDownFrame,recoilFrame,gunRecoilAngle;
    public int currentAmmo, dodgeStep, dodgeTime,exitVelocity , dodgeCooldown , dodgeDistance , flipWeapon , currentiFrames , iFrames ,shotCount , shotSpreadAngle ,shellAmount;
    public int railgunCharge, railgunChargeThres;
    public GameObject railShot;
    public bool isDevastator , chargeHeld;
    public int plyHP , plyRoundCounter ,maxRounds ,maxHP;
    public Color gunFireColour = new Color(1f, 1f, 0.585f);
    public Color railFireColour = new Color(1f, 0, 0);
    public Color rocketFireColour = new Color(1f, 1f, 0.585f);
    public CameraShakeInstance shook;

    [Header("Movemnent")]
    public bool stuck = false;



    [Header("Weapon generation")]
    public GameObject weaponPickup;
    public int weptype, weparchetype;
    public WeaponController.weapon currentWeapon = new WeaponController.weapon();
    public enum plyWeapState { hasWeapon, tossedWeapon };
    public plyWeapState plyState;
    public int tossedThreshold, tossedTime;
    public GameObject pistCasing , rockCasing , rifleCasing , shotCasing;
    public GameObject rocketPrefab;
    public GameObject autopist, smg, minigun, pumpShot, autoShot, multiShot, slowRPG, fastRPG, devastator, rifle, lmg, sniper ,weaponContainer , handcannon;
    public GameObject tossedWeapon;
    public Sprite previousSprite;
    public int gunDamage,newGunsCooldown,newCooldownCount;
    public bool canSpawnNewGun;

    public GameObject deathAnimSpriteCont ,underShadow , respawnPoint;


    public int totalCycleTime,ejectFrames,cycleFrames,elapseCycleTime;
    // Start is called before the first frame update
    void Awake()
    {
        plycont = this;
    }

    void Start()
    {
        baseAngle = -90;
        rb = this.GetComponent<Rigidbody>();
        dodge = false;
        finalDeath = false;
        plyDied = false;

        Instantiate(weaponPickup, this.transform.position, Quaternion.identity);
        Instantiate(weaponPickup, this.transform.position, Quaternion.identity);



    }

    // Update is called once per frame
    void Update()
    {
        if(finalDeath)
        {
            SceneManager.LoadScene(0);
        }

        if(EnemyAi.enAI.finalHP != 0)
        {
            if (EnemyAi.enAI.finalHP == 0)
            {
                crosshair.transform.position = new Vector3(1111, 1111, 1111);
            }

            if (plyHP > 0)
            {
                handleStates();
            }
            else
            {
                if (plyRoundCounter <= maxRounds)
                {
                    plyRoundCounter++;
                    plyHP = maxHP;
                    plyDied = true;

                }
                else
                {
                    finalDeath = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                EnemyAi.enAI.finalHP = 0;
            }

            if (plyReload.reCont.ammoPool > 0)
            {
                canSpawnNewGun = true;
            }

            if (plyReload.reCont.ammoPool == 0)
            {
                if (canSpawnNewGun)
                {
                    GameObject pickup = Instantiate(weaponPickup, this.transform.position, Quaternion.identity);
                    GameObject pickup2 = Instantiate(weaponPickup, this.transform.position, Quaternion.identity);
                    canSpawnNewGun = false;
                }



            }

            handleDeathUI();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();

        }
    }

    void handleDeathUI()
    {
        if(plyDied)
        {
            deathAnimSpriteCont.GetComponent<SpriteRenderer>().enabled = true;
            deathAnimSpriteCont.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            underShadow.GetComponent<SpriteRenderer>().enabled = true;
            weapon.transform.localPosition = new Vector3(0, 0, 10);
        }
        else
        {
            deathAnimSpriteCont.GetComponent<SpriteRenderer>().enabled = false;
            deathAnimSpriteCont.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            underShadow.GetComponent<SpriteRenderer>().enabled = false;
            weapon.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    void handleWeaponRotation()
    {
        if(EnemyAi.enAI.finalHP != 0)
        {
            xVal = Input.GetAxisRaw("Horizontal");
            yVal = Input.GetAxisRaw("Vertical");

            moveVect = new Vector2(xVal, yVal);



            ghostPly.transform.position = (moveVect.normalized * dodgeDistance) + this.transform.position;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            dir = mousePos - this.transform.position;
            bulletAngle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;


            crosshair.transform.position = new Vector3(mousePos.x, mousePos.y, 1);

            if (moveVect.magnitude == 0)
            {
                visGhost = false;
            }
            else
            {
                visGhost = true;
            }


            if (bulletAngle < 0)
            {
                weapSpr.GetComponent<SpriteRenderer>().flipY = false;
                wepBarrel.transform.localPosition = new Vector3(wepBarrel.transform.localPosition.x, 1.15f, 0);

            }
            else
            {
                weapSpr.GetComponent<SpriteRenderer>().flipY = true;
                wepBarrel.transform.localPosition = new Vector3(wepBarrel.transform.localPosition.x, -0.8f, 0);

            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                GameObject pickup = Instantiate(weaponPickup, this.transform.position, Quaternion.identity);

            }

          

            if (Input.GetKeyDown(KeyCode.O))
            {
                DebugSpawnWeapon();
            }

            weapon.transform.rotation = Quaternion.Euler(0, 0, -bulletAngle);
        }
       
    }

    void handleMouse()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            triggerDown = true;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            triggerDown = true;
            
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            triggerDown = false;
        }

        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            if(moveVect.magnitude != 0)
            {
                if (!dodge && (dodgeTime >= dodgeCooldown))
                {
                    dodgeTime = 0;
                    dodge = true;
                    Instantiate(warpAnim, this.transform.position, Quaternion.identity);
                    GameObject zap = Instantiate(zapAnim, this.transform.position, Quaternion.identity);
                    zap.transform.position = this.transform.position;
                    Vector3 warpDir = this.transform.position - ghostPly.transform.position  ;
                    float teleRot = Mathf.Atan2(warpDir.x, warpDir.y) * Mathf.Rad2Deg;
                    Debug.Log(teleRot);
                    zap.transform.localRotation = Quaternion.Euler(0, 0, -teleRot+90);

                }
            }
          
        }

        if(dodge)
        {
            invincible = true;
            ply.transform.position = (moveVect.normalized * dodgeDistance) + this.transform.position;
            dodge = false;
        }

        if(!dodge)
        {
            dodgeTime += 1;
        }

        handleIFrames();       
        if(!plyDied)
        {
            fireWeapon();

        }
}

    void handleIFrames()
    {
        if(invincible)
        {
            currentiFrames++;
            if(currentiFrames > iFrames)
            {
                currentiFrames = 0;
                invincible = false;
            }
        }
    }

    void fireWeapon()
    {
        if (weptype == 4)
        {
            if (weparchetype == 3)
            {
                isDevastator = true;
            }
            else
            {
                isDevastator = false;
            }
        }
        else
        {
            isDevastator = false;
        }


        if (!isDevastator)
        {
            if (triggerDown && shootReady)
            {
                if (currentAmmo > 0)
                {
                    if (plyReload.reCont.state == plyReload.reloadState.reloadBase)
                    {
                        currentAmmo--;

                        GameObject _muzzleFlash = Instantiate(muzzleFlash, wepBarrel.transform.position, Quaternion.identity);

                        _muzzleFlash.transform.parent = wepBarrel.transform;
                        _muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        _muzzleFlash.transform.GetChild(0).GetComponent<SpriteRenderer>().color = gunFireColour;
                        _muzzleFlash.transform.GetChild(1).GetComponent<SpriteRenderer>().color = gunFireColour;

                        switch (weptype)
                        {
                            case 1:
                                bulletController.bCont.SpawnProjectiles(shotCount, exitVelocity, 0.1f, wepBarrel.transform.position, bulletAngle + gunRecoilAngle, shotSpreadAngle, true, bulletPrefab);

                                break;
                            case 2:
                                bulletController.bCont.SpawnProjectiles(shotCount, exitVelocity, 0.1f, wepBarrel.transform.position, bulletAngle + gunRecoilAngle, shotSpreadAngle, true, bulletPrefab);
                                break;
                            case 3:
                                bulletController.bCont.SpawnProjectiles(shotCount, exitVelocity, 0.1f, wepBarrel.transform.position, bulletAngle + gunRecoilAngle, shotSpreadAngle, true, bulletPrefab);
                                break;
                            case 4:
                                if (weparchetype != 3)
                                {
                                   

                                    bulletController.bCont.SpawnProjectiles(1, 15, 0.1f, wepBarrel.transform.position, bulletAngle + gunRecoilAngle, shotSpreadAngle, true, rocketPrefab);
                                }
                                break;

                            case 5:
                                bulletController.bCont.SpawnProjectiles(shotCount, exitVelocity, 0.1f, wepBarrel.transform.position, bulletAngle + gunRecoilAngle, shotSpreadAngle, true, bulletPrefab);
                                break;

                        }

                        shootReady = false;
                        doRecoil = true;
                        recoilUp = true;
                    }

                }

            }
        }
        else
        {
            if (triggerDown && shootReady)
            {
                if (currentAmmo > 0)
                {
                    if(plyReload.reCont.state == plyReload.reloadState.reloadBase)
                    {
                     
                        railgunCharge++;
                        if (railgunCharge > railgunChargeThres)
                        {
             
                            currentAmmo--;

                            Instantiate(railShot, wepBarrel.transform.position, Quaternion.identity);

                            GameObject _muzzleFlash = Instantiate(muzzleFlash, wepBarrel.transform.position, Quaternion.identity);
                            _muzzleFlash.transform.parent = wepBarrel.transform;
                            _muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            _muzzleFlash.transform.GetChild(0).GetComponent<SpriteRenderer>().color = railFireColour;
                            _muzzleFlash.transform.GetChild(1).GetComponent<SpriteRenderer>().color = railFireColour;

                            shootReady = false;
                            doRecoil = true;
                            recoilUp = true;
                        }
                    }
                  
                }
            }
            else
            {
                if(railgunCharge > 0)
                {
                    railgunCharge--;
                }
                

            }

        }
        
    }

    void handleRecoil()
    {
     
        if(EnemyAi.enAI.finalHP != 0)
        {
            if (doRecoil)
            {
                if (recoilUp)
                {
                    recoilFrame++;

                    upRecoilStep = (recoilFrame / recoilUpFrame) * maxRecoilAngle;
                    currentRecoilDev = baseAngle - upRecoilStep;

                    gunRecoilAngle = Random.Range(-2f, upRecoilStep);

                    if (recoilFrame >= recoilUpFrame)
                    {
                        recoilFrame = recoilDownFrame;
                        recoilUp = false;
                    }
                }
                else
                {

                    recoilFrame--;
                    downRecoilStep = (recoilFrame / recoilDownFrame) * maxRecoilAngle;
                    currentRecoilDev = baseAngle - downRecoilStep;

                    gunRecoilAngle = Random.Range(-2f, downRecoilStep);

                    if (recoilFrame == 0)
                    {

                        doRecoil = false;
                    }

                }

                if (recoilDownFrame >= recoilUpFrame)
                {
                    recoilFrame = Mathf.Clamp(recoilFrame, 0, recoilDownFrame);
                }
                else
                {
                    recoilFrame = Mathf.Clamp(recoilFrame, 0, recoilUpFrame);
                }


                if (bulletAngle < 0)
                {
                    weapSpr.transform.localRotation = Quaternion.Euler(0, 0, currentRecoilDev);
                }
                else
                {
                    weapSpr.transform.localRotation = Quaternion.Euler(0, 0, -currentRecoilDev + 180);
                }

            }
        }
      
    }

    void GenerateWeapon()
    {
        switch (weptype)
        {
            case 1:
                currentWeapon.type = WeaponController.weaponType.pistol;
                break;
            case 2:
                currentWeapon.type = WeaponController.weaponType.shotgun;
                break;
            case 3:
                currentWeapon.type = WeaponController.weaponType.smg;
                break;
            case 4:
                currentWeapon.type = WeaponController.weaponType.rpg;
                break;
            case 5:
                currentWeapon.type = WeaponController.weaponType.rifle;
                break;

        }

        switch (weparchetype)
        {
            case 1:
                currentWeapon.archetype = WeaponController.weaponArchetype.highfire;
                break;
            case 2:
                currentWeapon.archetype = WeaponController.weaponArchetype.slowfire;
                break;
            case 3:
                currentWeapon.archetype = WeaponController.weaponArchetype.special;
                break;
        }

        reconfigureWeapon();

        currentWeapon.buildWeapon();
    }
       
    void reconfigureWeapon()
    {

        switch (weptype)
        {
            case 1:
                switch (weparchetype)
                {
                    case 1:
                        weaponContainer = autopist;
                        plyReload.reCont.ammoPool = 30;
                        break;
                    case 2:
                        weaponContainer = autopist;
                        plyReload.reCont.ammoPool = 20;
                        break;
                    case 3:
                        weaponContainer = handcannon;
                        plyReload.reCont.ammoPool = 15;
                        break;
                }
                break;
            case 2:
                switch (weparchetype)
                {
                    case 1:
                        weaponContainer = autoShot;
                        plyReload.reCont.ammoPool = 20;
                        break;
                    case 2:
                        weaponContainer = pumpShot;
                        plyReload.reCont.ammoPool = 15;
                        break;
                    case 3:
                        weaponContainer = multiShot;
                        plyReload.reCont.ammoPool = 18;
                        break;
                }
                break;
            case 3:
                switch (weparchetype)
                {
                    case 1:
                        weaponContainer = smg;
                        plyReload.reCont.ammoPool = 60;
                        break;
                    case 2:
                        weaponContainer = smg;
                        plyReload.reCont.ammoPool = 45;
                        break;
                    case 3:
                        weaponContainer = minigun;
                        plyReload.reCont.ammoPool = 100;
                        break;
                }
                break;
            case 4:
                switch (weparchetype)
                {
                    case 1:
                        weaponContainer = slowRPG;
                        plyReload.reCont.ammoPool = 10;
                        break;
                    case 2:
                        weaponContainer = fastRPG;
                        plyReload.reCont.ammoPool = 8;
                        break;
                    case 3:
                        weaponContainer = devastator;
                        plyReload.reCont.ammoPool = 0;
                        break;
                }
                break;
            case 5:
                switch (weparchetype)
                {
                    case 1:
                        weaponContainer = rifle;
                        plyReload.reCont.ammoPool = 30;
                        break;
                    case 2:
                        weaponContainer = lmg;
                        plyReload.reCont.ammoPool = 50;
                        break;
                    case 3:
                        weaponContainer = sniper;
                        plyReload.reCont.ammoPool = 15;
                        break;
                }
                break;
        }

        weapSpr.GetComponent<SpriteRenderer>().sprite = weaponContainer.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        weapSpr.transform.localPosition = weaponContainer.transform.GetChild(0).transform.localPosition;
        weapSpr.transform.localScale = weaponContainer.transform.GetChild(0).localScale;
        wepBarrel.transform.localPosition = weaponContainer.transform.GetChild(0).transform.GetChild(0).localPosition ;
        ejectionPort.transform.localPosition = weaponContainer.transform.GetChild(0).transform.GetChild(1).localPosition;
        plyReload.reCont.magPort.transform.localPosition = weaponContainer.transform.GetChild(0).transform.GetChild(2).localPosition;

    }




    void FixedUpdate()
    {
        if(EnemyAi.enAI.finalHP != 0)
        {
            if (!plyDied)
            {
                if (!stuck)
                {
                    if (!dodge)
                    {
                        rb.velocity = moveVect * moveSpeed; //moves ply
                    }

                }
            }
            else
            {
                rb.velocity = moveVect * (moveSpeed * 0.8f); //moves ply
            }


            handleRecoil();

            totalCycleTime = ejectFrames + cycleFrames;

            if (!shootReady)
            {
                elapseCycleTime++;

                if (elapseCycleTime == ejectFrames)
                {
                    switch (weptype)
                    {
                        case 1:
                            if (weparchetype != 3)
                            {
                                bulletController.bCont.SpawnProjectiles(shellAmount, 130, 0.1f, ejectionPort.transform.position, Random.Range(-5, 5), 20, true, pistCasing);
                            }
                            break;
                        case 2:
                            if (weparchetype != 3)
                            {
                                bulletController.bCont.SpawnProjectiles(1, 130, 0.1f, ejectionPort.transform.position, Random.Range(-5, 5), 20, true, shotCasing);
                            }

                            break;
                        case 3:
                            bulletController.bCont.SpawnProjectiles(shellAmount, 130, 0.1f, ejectionPort.transform.position, Random.Range(-5, 5), 20, true, pistCasing);
                            break;
                        case 4:
                            if (weparchetype == 3)
                            {
                                bulletController.bCont.SpawnProjectiles(1, 130, 0.1f, ejectionPort.transform.position, Random.Range(-5, 5), 20, true, rockCasing);
                            }

                            break;
                        case 5:
                            if (weparchetype != 3)
                            {
                                bulletController.bCont.SpawnProjectiles(shellAmount, 130, 0.1f, ejectionPort.transform.position, Random.Range(-5, 5), 20, true, rifleCasing);
                            }

                            break;


                    }

                }

                if (elapseCycleTime == totalCycleTime)
                {
                    elapseCycleTime = 0;
                    shootReady = true;
                }
            }
        }
     

    }


    void OnTriggerEnter(Collider col)
    {
        if(!plyDied)
        {
          //  Debug.Log(col);
            switch (col.gameObject.tag)
            {
                case "weaponPickup":
                    if (col.gameObject.GetComponent<weaponPickupBehaviour>().active)
                    {
                        invincible = true;
                        previousSprite = weapSpr.GetComponent<SpriteRenderer>().sprite;
                        weapon.transform.localPosition = new Vector3(0, 0, 10);
                        bulletController.bCont.SpawnProjectiles(1, 3500, 0.1f, wepBarrel.transform.position, bulletAngle, 1, true, tossedWeapon);


                        weptype = col.GetComponent<weaponPickupBehaviour>().weptype;
                        weparchetype = col.GetComponent<weaponPickupBehaviour>().weparchetype;
                        GenerateWeapon();
                        col.GetComponent<weaponPickupBehaviour>().DestroySelf();
                        plyState = plyWeapState.tossedWeapon;
                    }
                    break;
                case "EnemyBullet":
                    invincible = true;
                    bool _ignore = false;
                    plyHP -= 1;
                    if(col.gameObject.GetComponent<OrbitalLaserBehaviour>())
                    {
                        _ignore = true;
                    }
                    if (col.gameObject.GetComponent<sweepBehaviour>())
                    {
                        _ignore = true;
                    }
                    if (!_ignore)
                    {
                        Destroy(col.gameObject);
                    }

                    CameraShaker.Instance.ShakeOnce(15f, 6f, .1f, 1f);
                    break;
            }
        }
      
    }

    void handleStates()
    {
        switch (plyState)
        {
            case plyWeapState.hasWeapon:
                handleWeaponRotation();
                handleMouse();
                break;
            case plyWeapState.tossedWeapon:
                handleWeaponRotation();
                tossedTime++;
              

                if (tossedTime > tossedThreshold)
                {
                    weapon.transform.localPosition = new Vector3(0, 0, 0);
                    tossedTime = 0;

                    ejectFrames = currentWeapon.ejectFrames;
                    cycleFrames = currentWeapon.cycleFrames;
                    maxRecoilAngle = currentWeapon.maxRecoil;
                    recoilUpFrame = currentWeapon.upRecoilFrames;
                    recoilDownFrame = currentWeapon.downRecoilFrames;

                    exitVelocity = currentWeapon.exitVelocity;
                    plyReload.reCont.magSize = currentWeapon.magSize;
                    plyReload.reCont.elapsedFrames = 0;
                    plyReload.reCont.reloadTime = currentWeapon.totReloadFrames;
                    elapseCycleTime = 0;
                    shellAmount = currentWeapon.shellAmount;
                    shotCount = currentWeapon.shotCount;
                    currentAmmo = currentWeapon.magSize;
                   // cycleType = currentWeapon.cycleType;
                    shotSpreadAngle = currentWeapon.shotSpreadAngle;
                    gunDamage = currentWeapon.damage;

                    plyState = plyWeapState.hasWeapon;

                }
                break;
        }
    } 

    void DebugSpawnWeapon()
    {
        GenerateWeapon();

        ejectFrames = currentWeapon.ejectFrames;
        cycleFrames = currentWeapon.cycleFrames;
        maxRecoilAngle = currentWeapon.maxRecoil;
        recoilUpFrame = currentWeapon.upRecoilFrames;
        recoilDownFrame = currentWeapon.downRecoilFrames;

        exitVelocity = currentWeapon.exitVelocity;
        plyReload.reCont.magSize = currentWeapon.magSize;
        plyReload.reCont.elapsedFrames = 0;
        plyReload.reCont.reloadTime = currentWeapon.totReloadFrames;
        shellAmount = currentWeapon.shellAmount;
        shotCount = currentWeapon.shotCount;
        currentAmmo = currentWeapon.magSize;
        // cycleType = currentWeapon.cycleType;
        shotSpreadAngle = currentWeapon.shotSpreadAngle;
        gunDamage = currentWeapon.damage;
    }

    public void playerDied(string deathcause)
    {
        if(deathcause == "hole")
        {
            ply.transform.position = respawnPoint.transform.position;
            ply.transform.localScale = new Vector3(1, 1, 1);
            stuck = false;
        }
    }

}
