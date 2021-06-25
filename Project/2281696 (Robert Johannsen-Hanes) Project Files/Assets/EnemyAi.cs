using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyAi : MonoBehaviour
{
    public GameObject enemyBulletPrefab , orbital , bossBarHpCont,shield,smile;
    public int shootTime, shootThreshold , mod , mod2  ,shootTime2, shoot2Threshold ,reloadStep , reloadTime , shotsPerBarrage , currentShots ,phase;
    public float plyAngle, modAngle, orbAngle;
    public bool goUp,enemyAlive,Invulnerable,startupDebounce,timeOut;
    public enum enemyState { shoot , reload };
    public enemyState state;
    public enum enemymoveState { movetoTarget, sit , jiggle ,chase};
    public enemymoveState movestate;
    public GameObject eAmmoBarBase, eAmmoPointer;
    public TextMeshPro text;
    public int damageCount , maxHP, currentHP;
    public int startUptimethreshold, startupCount, cooldownTime, cooldownThreshold, previousphase, newphase ,chaseLag , chaselagCount ;
    public float moveSpeed,mod2Angle,mod4angle;
    public static EnemyAi enAI;

    public int P1ShotBarrageCount,P1At1Count;
    public int p1b2corner,p1b2barragestate,p1b2cooldownTime, p1b2cooldownThres ,p1b2shootTime2,p1b2shootThres2,p1b2shotCount,zigBarrageCount,p1b3shootTime,p1b3shootThres,p1b3count,p1b3state,p1b2autoTime,p1b2autoThres,p1b2autoCount;
    public bool p1debouncemid, debouncecornerroll, debouncep2intro, p2HideBoss, startShoot;


    public int p2b1shootTime, p2b1shootThres, p2b1cooldownTime, p2b1cooldownThres ,p2b1barrageCount,p2b1cycleCount;
    public bool p2b1debounce,goLeft;

    public int p2b2shootTime, p2b2shootThres, p2b2cooldownTime, p2b2cooldownThres, p2b2barrageCount ,p2b2state, _rollupordown, p2b2cycleCount,p2b2mod;

    public int p2b3cooldownTime, p2b3cooldownThres;

    public int finalHP = 1;
  

    public enum phase1state { barrage1,barrage2,barrage3 };
    public phase1state p1state;

    public enum phase2state { introAnim ,barrage1, barrage2, barrage3 };
    public phase2state p2state;
    public TextMeshPro bossDlgBox;
    public float _animTime, _animThres;
    public int animPart;
    public bool mitosis, mitosisSpreadOut , mitosisShoot , mitosisCombine,mitosisDie;
    public Vector3 mitosisPos;

    public enum phase3state { introAnim, barrage1, barrage2, barrage3 };
    public phase3state p3state;

    public int p3b1shootTime, p3b1shootThres, p3b1cooldownTime, p3b1cooldownThres, p3b1barrageCount, p3b1cycleCount,p3b2roll;

    public int p3b3shootTime, p3b3shootThres, p3b3cooldownTime, p3b3cooldownThres, p3b3barrageCount, p3b3cycleCount, p3b3roll;


    public GameObject mid, leftmid, rightmid, topmid, topleft, topright,bottommid, bottomleft, bottomright , outtopmid , outtopleft , outtopright , outbotmid , outbotleft ,outbotright;
    public Vector3 targetpos;

    [Header("AttackContainer")]
    public bool LongAttDebounce;
    public GameObject orbitalStrikeLong,P1fastShot,autoTrack,groundCrack,p2Boss,bossSplit,sweep,finalStand,holeMaker,squib;

    public SpriteRenderer plysp, bosssp, gunsp, backgroundsp, dominated , topholesp;
    public Color backcolor ,domcolor ;
    public TextMeshPro overkill;
    public GameObject gun,particles;

    void Awake()
    {
        enAI = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        currentShots = shotsPerBarrage;
        goUp = true;
        phase = 1;
        enemyAlive = false;
        startupDebounce = true;
        targetpos = mid.transform.position;
        moveSpeed = 0.5f;
        chaseLag = 2;
        LongAttDebounce = false;
        debouncecornerroll = true;
        debouncep2intro = true;
        p1b2autoCount = 0;
        p2b1cycleCount = 0;
        p2b1barrageCount = 0;

    }

    void Update()
    {
      
        if(finalHP == 1)
        {
            if (startupCount < startUptimethreshold)
            {
                startupCount++;

            }
            else
            {
                if (startupDebounce)
                {
                    enemyAlive = true;
                    startupCount = 0;
                    startupDebounce = false;
                    Invulnerable = false;
                }
            }



            shield.GetComponent<SpriteRenderer>().enabled = Invulnerable;


            currentShots = Mathf.Clamp(currentShots, 0, 999);
            //  text.text = currentShots + " ";

            if (Input.GetKeyDown(KeyCode.Z))
            {
                enemyAlive = !enemyAlive;

            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                phase = 2;

            }
        }
        else
        {

            bossBarHpCont.transform.position = new Vector3(1111, 1111, 1111);
            plysp.color = domcolor;
            bosssp.color = domcolor;
            Destroy(gun);
            backgroundsp.enabled = true;
            dominated.enabled = true;
            overkill.GetComponent<TextMeshPro>().enabled = true;
            topholesp.color = backcolor;
            particles.transform.position = new Vector3(particles.transform.position.x, particles.transform.position.y, 0);
        }
     


     

        if(plyCont.plycont.plyDied)
        {
            timeOut = true;
        }

        if(timeOut)
        {
            cooldownTime++; //timeout after every death
            if(cooldownTime > cooldownThreshold)
            {
                cooldownTime = 0;
                timeOut = false;
                plyCont.plycont.plyDied = false;
                if(plyCont.plycont.plyRoundCounter != plyCont.plycont.maxRounds)
                {
                    bossDlgBox.text = "ITS ROUND " + plyCont.plycont.plyRoundCounter;
                }
                else
                {
                    bossDlgBox.text = "FINAL ROUND!!!! ";
                }
              
            }
        }

        UpdateBossHPBar();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(enemyAlive)
        {
            switch(movestate)
            {
                case enemymoveState.movetoTarget:
                   
                        this.transform.position = Vector3.MoveTowards(this.transform.position, targetpos, moveSpeed);
                   
                 
                  
                    break;
                case enemymoveState.sit:
                    break;
                case enemymoveState.chase:
                    chaselagCount++;
                    if (chaselagCount >= chaseLag)
                    {
                        chaselagCount = 0;
                      
                            this.transform.position = Vector3.MoveTowards(this.transform.position, plyCont.plycont.ply.transform.position, moveSpeed);
                        
                        
                    
                        
                   

                    }
                   
                    break;
            }
           

            if(this.transform.position == targetpos)
            {
                if(!LongAttDebounce)
                {
                    LongAttDebounce = true;
                   

                    Instantiate(plyCont.plycont.weaponPickup, this.transform.position, Quaternion.identity);
                    Instantiate(plyCont.plycont.weaponPickup, this.transform.position, Quaternion.identity);
                    Instantiate(plyCont.plycont.weaponPickup, this.transform.position, Quaternion.identity);
                    Instantiate(plyCont.plycont.weaponPickup, this.transform.position, Quaternion.identity);
                    movestate = enemymoveState.sit;
                }

                startShoot = true;
              
              

               
            }

            if(startShoot)
            {
                bulletPatterns();
            }
         
        }
      
    }

    void bulletPatterns()
    {
        if (!timeOut)
        {
            switch (state)
            {
                case enemyState.shoot:

                    Vector3 dir = plyCont.plycont.ply.transform.position - this.transform.position;
                    plyAngle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
                    handleShoot();
                    
               

                    break;
                case enemyState.reload:
                    
                    eAmmoPointer.transform.localPosition = new Vector3(-0.5f + ((reloadStep - reloadTime) / reloadTime), eAmmoPointer.transform.localPosition.y, eAmmoPointer.transform.localPosition.z);
                    break;

            }
        }
    }

    void handleShoot()
    {
        switch (phase)
        {
            case 1:
                switch (p1state)
                {
                    case phase1state.barrage1:
                        p1Barrage1();
                        break;
                    case phase1state.barrage2:
                        p1barrage2();
                        break;
                    case phase1state.barrage3:
                        p1barrage3();
                        break;
                }
                break;
            case 2:
                switch (p2state)
                {
                    case phase2state.introAnim:
                        p2IntroAnim();
                        break;
                    case phase2state.barrage1:
                        p2Barrage1();
                        break;
                    case phase2state.barrage2:
                        p2Barrage2();
                        break;
                    case phase2state.barrage3:
                        p2Barrage3();
                        break;
                }
                break;
            case 3:
                switch (p3state)
                {
                    case phase3state.introAnim:
                        p3IntroAnim();
                        break;
                    case phase3state.barrage1:
                        p3barrage1();
                        break;
                    case phase3state.barrage2:
                        p3barrage2();
                        break;
                    case phase3state.barrage3:
                        p3barrage3();
                        break;
                }
                break;
            case 4:
                p4barrage1();
                break;

        }
    }

    void p1Barrage1()
    {
        if(P1At1Count < 4)
        {
            if (currentShots > 0)
            {
                shootTime++;
                shootTime2++;
              

                if (shootTime2 >= shoot2Threshold)
                {

                    shootTime2 = 0;

                    if (P1ShotBarrageCount > 2)
                    {
                        P1ShotBarrageCount = 0;
                        mod2Angle = plyAngle;
                    }

                    P1ShotBarrageCount += 1;





                    bulletController.bCont.SpawnProjectiles(3, 25, 1, this.transform.position, mod2Angle, 25, true, P1fastShot);
                }

                if (shootTime >= shootThreshold)
                {
                    shootTime = 0;
                    mod++;


                    if (mod >= 12)
                    {
                        mod = 0;
                    }

                    currentShots -= 20;
                    modAngle = (mod * 10) + plyAngle;
                    bulletController.bCont.SpawnProjectiles(20, 3, 1, this.transform.position, modAngle, 360, true, enemyBulletPrefab);
                }

            }
            else
            {
                reloadStep++;
                if (reloadStep >= reloadTime)
                {
                    P1At1Count++;
                    currentShots = shotsPerBarrage;
                    reloadStep = 0;

                }

            }
        }
        else
        {
            P1At1Count = 0;
            p1state = phase1state.barrage2;
        }
       
    }


    void p1barrage2()
    {
        if (p1b2barragestate == 0)
        {
          

            if (debouncecornerroll)
            {
                p1b2corner = Random.Range(0, 5);
                moveSpeed = 0.3f;
                debouncecornerroll = false;

                switch (p1b2corner)
                {
                    case 1:
                        targetpos = topleft.transform.position;
                        break;
                    case 2:
                        targetpos = topright.transform.position;
                        break;
                    case 3:
                        targetpos = bottomleft.transform.position;
                        break;
                    case 4:
                        targetpos = bottomright.transform.position;
                        break;
                }
            }

            if(p1b2autoCount < 5)
            {
                p1b2autoTime++;
                if (p1b2autoTime > p1b2autoThres)
                {
                    p1b2autoCount++;
                    p1b2autoTime = 0;
                    Instantiate(autoTrack, this.transform.position, Quaternion.identity);
                }

            }



            movestate = enemymoveState.movetoTarget;

            if (this.transform.position == targetpos)
            {
                movestate = enemymoveState.sit;
                if (p1b2barragestate == 0)
                {
                    p1b2shootTime2 = 0;
                    p1b2barragestate += 1;
                }

            }


          

         
        }
       

        if(movestate == enemymoveState.sit)
        {
            switch(p1b2barragestate)
            {
                case 1:
                    if(zigBarrageCount < 3)
                    {
                        if(p1b2shotCount < 100)
                        {
                            p1b2shootTime2++;
                            if (p1b2shootTime2 >= p1b2shootThres2)
                            {
                                p1b2shootTime2 = 0;

                                if (goUp)
                                {
                                    mod2++;
                                }
                                else
                                {
                                    mod2--;
                                }

                                if (mod2 > 5)
                                {
                                    goUp = false;
                                }

                                if (mod2 < 0)
                                {
                                    goUp = true;
                                }

                                float mod2Angle = plyAngle + (mod2 * 7);
                                p1b2shotCount++;
                              
                                bulletController.bCont.SpawnProjectiles(1, 10, 1, this.transform.position, mod2Angle, 10, false, enemyBulletPrefab);

                            }
                        }
                        else
                        {
                            if (p1b2cooldownTime <= p1b2cooldownThres)
                            {
                                p1b2cooldownTime++;
                            }
                            else
                            {
                                p1b2shotCount = 0;
                                zigBarrageCount++;
                                p1b2cooldownTime = 0;

                            }
                        }
                    }
                    else
                    {
                        movestate = enemymoveState.movetoTarget;
                        zigBarrageCount = 0;
                        p1b2barragestate = 0 ;
                        debouncecornerroll = true;
                        p1b2autoCount = 0;
                        p1state = phase1state.barrage3;
                    }
                   

                    break;
             
         

            }
        }
    }

    void p1barrage3()
    {
        switch (p1b3state)
        {
            case 0:
                moveSpeed = 1.5f;
                targetpos = outtopmid.transform.position;

                movestate = enemymoveState.movetoTarget;

                if (this.transform.position == targetpos)
                {
                    movestate = enemymoveState.sit;
                    
                        p1b2shootTime2 = 0;
                        p1b3state += 1;
                    

                }
                break;
            case 1:
                if (p1b3count <= 25)
                {
                    p1b3shootTime++;
                    if (p1b3shootTime >= p1b3shootThres)
                    {
                        p1b3shootTime = 0;


                        p1b3count++;
                        Instantiate(groundCrack, plyCont.plycont.ply.transform.position, Quaternion.identity);

                    }
                }
                else
                {
                    p1b3state++;
                    p1b3count = 0;
                    
                   
                    P1At1Count = 0;
                }
                break;
            case 2:
                if (p1b2cooldownTime <= p1b2cooldownThres)
                {
                    p1b2cooldownTime++;
                }
                else
                {
                    
                    
                    p1b2cooldownTime = 0;
                    p1b3state = 0;
                    p1state = phase1state.barrage1;
                    targetpos = mid.transform.position;
                    movestate = enemymoveState.movetoTarget;
                    

                }
                break;
            

        }
    }

    void p2IntroAnim()
    {
        if(debouncep2intro)
        {
            _animThres = 15f;
            
            debouncep2intro = false;
            Invulnerable = true;
            targetpos = mid.transform.position;
            movestate = enemymoveState.movetoTarget;
            

        }

        _animTime += Time.deltaTime;

        if(_animTime > 1f)
        {
            _animTime = 0;
            animPart++;
        }

        switch (animPart)
        {
            case 1:
                bossDlgBox.text = "Establishing Orbital Uplink";
                break;
            case 2:
                bossDlgBox.text = "Establishing Orbital Uplink.";
                break;
            case 3:
                bossDlgBox.text = "Establishing Orbital Uplink..";
                break;
            case 4:
                bossDlgBox.text = "Establishing Orbital Uplink...";
                break;
            case 5:
                bossDlgBox.text = "Establishing Orbital Uplink";
                break;
            case 6:
                bossDlgBox.text = "Establishing Orbital Uplink.";
                break;
            case 7:
                bossDlgBox.text = "Establishing Orbital Uplink..";
                break;
            case 8:
                bossDlgBox.text = "Establishing Orbital Uplink...";
                break;
            case 9:
                bossDlgBox.text = "";
                break;
            case 10:
                bossDlgBox.text = "";
                break;
            case 11:
                bossDlgBox.text = "-Established-";
                break;
            case 12:
                bossDlgBox.text = "Shot Ready in 3 [You]";
                break;
            case 13:
                bossDlgBox.text = "Shot Ready in 2 [Will]";
                break;
            case 14:
                bossDlgBox.text = "Shot Ready in 1 [DIE!]";
                break;
            case 15:
                bossDlgBox.text = "";
                p2HideBoss = true;
                
                bulletController.bCont.SpawnProjectiles(20, 0, .1f, this.transform.position, 0, 360, false, bossSplit);
                mitosisCombine = true;

                Invulnerable = false;
                this.GetComponent<SpriteRenderer>().enabled = false;
                GameObject _orbit = Instantiate(orbitalStrikeLong, topmid.transform.position, Quaternion.identity);
                _orbit.GetComponent<OrbitalLaserBehaviour>().LifeThreshold = 20;
                _orbit.GetComponent<OrbitalLaserBehaviour>().moveSpeed = 0.2f;
                _orbit.GetComponent<OrbitalLaserBehaviour>().chaseLag = 4;
                animPart++;
                break;
            case 16:
                movestate = enemymoveState.chase;
                p2state = phase2state.barrage1;
                animPart = 0;
                break;


        }

      
    }

    void p2Barrage1()
    {
        moveSpeed = 0.6f;
        if(p2b1cycleCount < 4)
        {
            p2b1cooldownTime++;
            if (p2b1cooldownTime >= p2b1cooldownThres)
            {
                movestate = enemymoveState.sit;
                if (p2b1debounce)
                {
                    p2b1debounce = false;
                    mitosisCombine = false;
                    mitosisSpreadOut = true;
                }

                p2b1shootTime++;

                if (p2b1barrageCount < 2)
                {
                    if (p2b1shootTime >= p2b1shootThres)
                    {
                        p2b1shootTime = 0;

                        mitosisShoot = true;
                        p2b1barrageCount++;


                    }
                    else
                    {
                        mitosisShoot = false;
                    }
                }
                else
                {
                    p2b1debounce = true;
                    movestate = enemymoveState.chase;
                    p2b1barrageCount = 0;
                    p2b1cycleCount++;
                    p2b1cooldownTime = 0;
                    mitosisCombine = true;
                    mitosisSpreadOut = false;
                }



            }
        }
        else
        {
            p2b1cycleCount = 0;

            _rollupordown = Random.Range(0, 3);
            if(_rollupordown == 1)
            {
                targetpos = outtopmid.transform.position;
            }
            else
            {
                targetpos = outbotmid.transform.position;
            }
            movestate = enemymoveState.movetoTarget;
            p2b2state = 0;
            p2state = phase2state.barrage2;
        }
      

       
        
    }

    void p2Barrage2()
    {
        moveSpeed = 0.5f;
        switch (p2b2state)
        {
            case 0:
                if(this.transform.position == targetpos)
                {
                    p2b2state++;
                    goLeft = true;

                    if(_rollupordown == 1)
                    {
                        targetpos = outtopleft.transform.position;
                    }
                    else
                    {
                        targetpos = outbotleft.transform.position;
                    }
                    
                }
                break;
            case 1:
                if(p2b2cycleCount < 5)
                {
                    if (this.transform.position == targetpos)
                    {
                        if (goLeft)
                        {
                            if (_rollupordown == 1)
                            {
                                targetpos = outtopright.transform.position;
                            }
                            else
                            {
                                targetpos = outbotright.transform.position;
                            }
                            p2b2cycleCount++;
                        }
                        else
                        {
                            if (_rollupordown == 1)
                            {
                                targetpos = outtopleft.transform.position;
                            }
                            else
                            {
                                targetpos = outbotleft.transform.position;
                            }
                            p2b2cycleCount++;
                        }
                    }

                    p2b2shootTime++;
                    if (p2b2shootTime >= p2b2shootThres)
                    {
                        p2b2shootTime = 0;

                        p2b2mod++;
                        bulletController.bCont.SpawnProjectiles(4, 95, .1f, this.transform.position, (plyAngle + p2b2mod), 360, true, enemyBulletPrefab);

                    }
                }
                else
                {
                    p2b2cycleCount = 0;
                    p2state = phase2state.barrage3;
                    moveSpeed = 0.3f;
                    targetpos = mid.transform.position;
                }
              
                break;
           
        }
    }

    void p2Barrage3()
    {
        bossDlgBox.text = "[YOU] [WILL] [DIE] [,AGAIN]";

        p2b3cooldownTime++;
        if(p2b3cooldownTime >= p2b3cooldownThres)
        {
            GameObject _orbit = Instantiate(orbitalStrikeLong, topleft.transform.position, Quaternion.identity);
            _orbit.GetComponent<OrbitalLaserBehaviour>().LifeThreshold = 10;
            _orbit.GetComponent<OrbitalLaserBehaviour>().moveSpeed = 0.05f;
            _orbit.GetComponent<OrbitalLaserBehaviour>().chaseLag = 6;

            GameObject _orbit2 = Instantiate(orbitalStrikeLong, bottomright.transform.position, Quaternion.identity);
            _orbit2.GetComponent<OrbitalLaserBehaviour>().LifeThreshold = 10;
            _orbit2.GetComponent<OrbitalLaserBehaviour>().moveSpeed = 0.05f;
            _orbit2.GetComponent<OrbitalLaserBehaviour>().chaseLag = 6;

            p2state = phase2state.barrage1;
        }


    }

    

    void p3IntroAnim()
    {
        _animTime += Time.deltaTime;

        if (_animTime > 1f)
        {
            _animTime = 0;
            animPart++;
        }


        switch(animPart)
        {
            case 0:
                mitosisDie = true;
                this.GetComponent<SpriteRenderer>().enabled = true;
                targetpos = rightmid.transform.position;
                movestate = enemymoveState.movetoTarget;
                Invulnerable = true;
                break;
            case 1:
               
                break;

            case 4:
                
                bossDlgBox.text = "DEATH";
                break;
            case 5:
                
                bossDlgBox.text = "death";
                break;
            case 6:
                
                bossDlgBox.text = "calls";
                break;
            case 7:
                
                bossDlgBox.text = "CALLS";
                break;
            case 8:

                bossDlgBox.text = "FOR";
                break;
            case 9:
                smile.GetComponent<SpriteRenderer>().enabled = true;
                bossDlgBox.text = "YOU";
                break;
            case 10:
                animPart = 0;
                p3state = phase3state.barrage1;
                break;
        }
    }

    void p3barrage1()
    {
        Invulnerable = false;
        if(p3b1barrageCount < 15)
        {
            p3b1shootTime++;
            if (p3b1shootTime >= p3b1shootThres)
            {
                p3b1barrageCount++;
                p3b1shootTime = 0;
                movestate = enemymoveState.sit;
                this.transform.position = new Vector3((((plyCont.plycont.ply.transform.position - this.transform.position).normalized * 10) + this.transform.position).x + Random.Range(-6, 6), (((plyCont.plycont.ply.transform.position - this.transform.position).normalized * 10) + this.transform.position).y + Random.Range(-6, 6), this.transform.position.z);
                Instantiate(autoTrack, this.transform.position, Quaternion.identity);
            }
        }
        else
        {
            p3b1barrageCount = 0;
            p3state = phase3state.barrage2;
            p3b2roll = Random.Range(0, 3);
            movestate = enemymoveState.movetoTarget;
            if(p3b2roll == 1)
            {
                targetpos = leftmid.transform.position;
            } 
            else
            {
                targetpos = rightmid.transform.position;
            }
        }
       
        
    }

    void p3barrage2()
    {
        if(this.transform.position == targetpos)
        {
            _animTime += Time.deltaTime;

            if (_animTime > 1f)
            {
                _animTime = 0;
                animPart++;
            }

            shootTime2++;


            if (shootTime2 >= shoot2Threshold)
            {

                shootTime2 = 0;

                if (P1ShotBarrageCount > 7)
                {
                    P1ShotBarrageCount = 0;
                    mod2Angle = plyAngle;
                }

                P1ShotBarrageCount += 1;





                bulletController.bCont.SpawnProjectiles(2, 25, 1, this.transform.position, mod2Angle, 45, true, P1fastShot);
            }

            switch (animPart)
            {
                case 0:

                    break;
                case 1:

                    break;
                case 2:

                    break;
                case 3:
                    
                    break;
                case 4:
                    Instantiate(sweep, mid.transform.position, Quaternion.identity);
                    animPart++;
                    break;
                case 28:
                    moveSpeed = 4f;
                    targetpos = outtopmid.transform.position;
                    movestate = enemymoveState.movetoTarget;
                    p3state = phase3state.barrage3;
                    break;

            }
        }
       
    }

    void p3barrage3()
    {
            if(this.transform.position == targetpos)
        {
            if(p3b3cycleCount <= 3)
            {
                if (p3b3barrageCount <= 10)
                {
                    p3b3shootTime++;


                    if (p3b3shootTime >= p3b3shootThres)
                    {
                        if (p3b3barrageCount%2 == 0)
                        {
                            
                            mod4angle = plyAngle;
                        }


                        p3b3shootTime = 0;
                        if (goUp)
                        {
                            mod2++;
                        }
                        else
                        {
                            mod2--;
                        }

                        if (mod2 > 5)
                        {
                            goUp = false;
                        }

                        if (mod2 < 0)
                        {
                            goUp = true;
                        }

                        float mod2Angle = mod4angle + (mod2 * 9);
                        float mod3Angle = mod4angle - (mod2 * 9);

                        p3b3barrageCount++;
                        bulletController.bCont.SpawnProjectiles(1, 15, 1, this.transform.position, mod4angle, 50, true, enemyBulletPrefab);
                        bulletController.bCont.SpawnProjectiles(1, 15, 1, this.transform.position, mod2Angle, 50, true, enemyBulletPrefab);
                        bulletController.bCont.SpawnProjectiles(1, 15, 1, this.transform.position, mod3Angle, 50, true, enemyBulletPrefab);
                        
                    }
                }
                else
                {
                    p3b3cooldownTime++;
                    if(p3b3cooldownTime >= p3b3cooldownThres)
                    {
                        p3b3cooldownTime = 0;
                        p3b3cycleCount++;
                        p3b3barrageCount = 0;
                    }
                  

                }
            }
            else
            {
                animPart = 0;
                p3b3cycleCount = 0;
                movestate = enemymoveState.chase;
                p3state = phase3state.barrage1;
            }
           
           

        }





    }

    void p4barrage1()
    {
        smile.GetComponent<SpriteRenderer>().enabled = false;
        targetpos = finalStand.transform.position;
        moveSpeed = 0.1f;
        movestate = enemymoveState.movetoTarget;

        if(this.transform.position == targetpos)
        {
            _animTime += Time.deltaTime;

            if (_animTime > 1f)
            {
                _animTime = 0;
                animPart++;
            }
            bossDlgBox.color = Color.white;

            switch (animPart)
            {
                case 0:
                    bossDlgBox.text = "DIE";

                 
                    break;
                case 2:
                    bossDlgBox.text = "die";

                    break;
                case 6:
                    bossDlgBox.text = "please die";

                    break;
                case 12:
                    bossDlgBox.text = "die ...";
                    break;
                case 16:
                    bossDlgBox.text = "...";
                    break;


            }
        }

        
    }


    void UpdateBossHPBar()
    {
       
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        float _maxHP = maxHP;
        float _BossHPPerc = currentHP / _maxHP;
        bossBarHpCont.transform.localScale = new Vector3(_BossHPPerc , bossBarHpCont.transform.localScale.y , 1);
        bossBarHpCont.transform.localPosition = new Vector3((_BossHPPerc*900)-900, bossBarHpCont.transform.localPosition.y, bossBarHpCont.transform.localPosition.z);

        if ((currentHP < (_maxHP/3)*2) ) //fucking bs unity pos looking ass why wont you accept my and statement
        {
            if((currentHP > _maxHP / 3))
            {
                phase = 2;
              
            }
       
        }

        if((currentHP < (_maxHP / 3)))
        {
            if(currentHP > 0)
            {
                phase = 3;
            }
           
        }
        if(currentHP == 0)
        {
            phase = 4;
        }

        previousphase = phase;
        if (previousphase > newphase)
        {
            Instantiate(plyCont.plycont.weaponPickup, this.transform.position, Quaternion.identity);
            Instantiate(plyCont.plycont.weaponPickup, this.transform.position, Quaternion.identity);
        }

        newphase = phase;
    }


    void OnTriggerEnter(Collider col)
    {
        if(!Invulnerable)
        {
           
            if (col.gameObject.tag == "bullet")
            {

                damageCount += plyCont.plycont.gunDamage;
                currentHP -= plyCont.plycont.gunDamage;

                if (phase == 4)
                {
                    GameObject _squib = Instantiate(squib, col.ClosestPointOnBounds(this.transform.position), Quaternion.identity);
                    _squib.transform.localRotation = Quaternion.Euler(0, 0, -plyCont.plycont.bulletAngle);
                    finalHP = 0;
                }

            }

            if(col.gameObject.tag == "rail")
            {
               
                if(col.gameObject.GetComponent<railShotBehaviour>().shotActive)
                {
                   
                    damageCount += plyCont.plycont.gunDamage;
                    currentHP -= plyCont.plycont.gunDamage;

                    if (phase == 4)
                    {
                        GameObject _squib = Instantiate(squib, col.ClosestPointOnBounds(this.transform.position), Quaternion.identity);
                            _squib.transform.localRotation = Quaternion.Euler(0, 0, plyCont.plycont.bulletAngle);
                        finalHP = 0;
                    }

                }
             
            }


        }
    }


}
