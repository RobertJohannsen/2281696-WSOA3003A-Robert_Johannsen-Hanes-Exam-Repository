using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public enum weaponType { pistol , shotgun , smg , rpg , rifle };
    public enum weaponArchetype { highfire , slowfire , special};
    public enum cycleType { auto , semi , pump , revolver ,rpg};

    public class weapon
    {
        public weaponType type;
        public weaponArchetype archetype;
        public int ejectFrames, cycleFrames, maxRecoil, upRecoilFrames, downRecoilFrames, exitVelocity;
        public int magSize, totReloadFrames, startFrames, reloadPercent;
        public int baseDamage , shotCount , shellAmount , shotSpreadAngle,damage;
        public string gunname;
        public cycleType cycleType;
        public GameObject shell , bulletType;

        public void buildWeapon()
        {
            int weaponRoll = Random.Range(1, 5);
            switch (archetype)
            {
                case weaponArchetype.highfire:
                    
                    switch (type)
                    {
                        case weaponType.pistol:
                            ejectFrames = 2;
                            cycleFrames = 3;
                            maxRecoil = 45;
                            upRecoilFrames = 1;
                            downRecoilFrames = 4;
                            exitVelocity = 850;
                            magSize = 17 + Random.Range(0, 5);
                            totReloadFrames = 100;
                            shellAmount = 1;
                            shotCount = 1;
                            cycleType = cycleType.auto;
                            shotSpreadAngle = 1;
                            damage = 1;
                            break;

                        case weaponType.shotgun:
                            ejectFrames = 12;
                            cycleFrames = 5;
                            maxRecoil = 42;
                            upRecoilFrames = 2;
                            downRecoilFrames = 6;
                            exitVelocity = 1250;
                            magSize = 20 + Random.Range(0, 7);
                            totReloadFrames = 150;
                            shellAmount = 1;
                            shotCount = 5;
                            cycleType = cycleType.auto;
                            shotSpreadAngle = 50;
                            damage = 3;
                            break;

                        case weaponType.smg:
                            ejectFrames = 2;
                            cycleFrames = 2;
                            maxRecoil = 40;
                            upRecoilFrames = 2;
                            downRecoilFrames = 5;
                            exitVelocity = 1250;
                            magSize = 45 + Random.Range(0, 7);
                            totReloadFrames = 160;
                            shellAmount = 1;
                             shotCount = 1;
                            cycleType = cycleType.auto;
                            shotSpreadAngle = 1;
                            damage = 1;
                            break;

                        case weaponType.rpg:
                            ejectFrames = 40;
                            cycleFrames = 40;
                            maxRecoil = 1;
                            upRecoilFrames = 2;
                            downRecoilFrames = 5;
                            exitVelocity = 3000;
                            magSize = 1;
                            totReloadFrames = 300;
                            shellAmount = 1;
                             shotCount = 1;
                            cycleType = cycleType.rpg;
                            shotSpreadAngle = 1;
                            damage = 40;
                            break;

                        case weaponType.rifle:
                            ejectFrames = 2;
                            cycleFrames = 7;
                            maxRecoil = 30;
                            upRecoilFrames = 2;
                            downRecoilFrames = 5;
                            exitVelocity = 2222;
                            magSize = 45 + Random.Range(0, 7);
                            totReloadFrames = 180;
                            shellAmount = 1;
                             shotCount = 1;
                            cycleType = cycleType.auto;
                            shotSpreadAngle = 1;
                            damage = 2;
                            break;
                    }
              


                    

                    break;
                case weaponArchetype.slowfire:

                    switch (type)
                    {
                        case weaponType.pistol:
                            ejectFrames = 5;
                            cycleFrames = 15;
                            maxRecoil = 75;
                            upRecoilFrames = 3;
                            downRecoilFrames = 15;
                            exitVelocity = 1500;
                            magSize = 10 + Random.Range(0, 5);
                            totReloadFrames = 95;
                            shellAmount = 1;
                            shotCount = 1;
                            cycleType = cycleType.auto;
                            shotSpreadAngle = 1;
                            damage = 3;
                            break;

                        case weaponType.shotgun:
                            ejectFrames = 45;
                            cycleFrames = 12;
                            maxRecoil = 90;
                            upRecoilFrames = 2;
                            downRecoilFrames = 20;
                            exitVelocity = 1250;
                            magSize = 9 + Random.Range(0, 5);
                            totReloadFrames = 170;
                            shellAmount = 1;
                            shotCount = 7;
                            cycleType = cycleType.pump;
                            shotSpreadAngle = 40;
                            damage = 3;
                            break;

                        case weaponType.smg:
                            ejectFrames = 4;
                            cycleFrames = 2;
                            maxRecoil = 40;
                            upRecoilFrames = 2;
                            downRecoilFrames = 5;
                            exitVelocity = 1500;
                            magSize = 20 + Random.Range(0, 7);
                            totReloadFrames = 120;
                            shellAmount = 1;
                             shotCount = 1;
                            cycleType = cycleType.auto;
                            shotSpreadAngle = 1;
                            damage = 1;
                            break;

                        case weaponType.rpg:
                            ejectFrames = 4;
                            cycleFrames = 4;
                            maxRecoil = 35;
                            upRecoilFrames = 2;
                            downRecoilFrames = 16;
                            exitVelocity = 2500;
                            magSize = 4 + Random.Range(0,3);
                            totReloadFrames = 350 ;
                            shellAmount = 1;
                             shotCount = 1;
                            cycleType = cycleType.rpg;
                            shotSpreadAngle = 1;
                            damage = 20;
                            break;

                        case weaponType.rifle:
                            ejectFrames = 8;
                            cycleFrames = 8;
                            maxRecoil = 40;
                            upRecoilFrames = 2;
                            downRecoilFrames = 16;
                            exitVelocity = 2500;
                            magSize = 20 + Random.Range(0, 7);
                            totReloadFrames = 170;
                            shellAmount = 1;
                             shotCount = 1;
                            cycleType = cycleType.auto;
                            shotSpreadAngle = 1;
                            damage = 4;
                            break;
                    }

                    break;
                case weaponArchetype.special:

                    switch (type)
                    {
                        case weaponType.pistol:
                            ejectFrames = 5;
                            cycleFrames = 32;
                            maxRecoil = 120;
                            upRecoilFrames = 2;
                            downRecoilFrames = 27;
                            exitVelocity = 2000;
                            magSize = 6 + Random.Range(0, 10);
                            totReloadFrames = 100;
                            shellAmount = 1;
                            shotCount = 1;
                            cycleType = cycleType.revolver;
                            shotSpreadAngle = 1;
                            damage = 10;
                            break;

                        case weaponType.shotgun:
                            ejectFrames = 1;
                            cycleFrames = 5;
                            maxRecoil = 85;
                            upRecoilFrames = 2;
                            downRecoilFrames = 20;
                            exitVelocity = 2000;
                            magSize = 2 + Random.Range(0,4);
                            totReloadFrames = 30;
                            shellAmount = magSize;
                            shotCount = 12;
                            cycleType = cycleType.revolver;
                            shotSpreadAngle = 50;
                            damage = 3;
                            break;

                        case weaponType.smg:
                            ejectFrames = 1;
                            cycleFrames = 1;
                            maxRecoil = 17;
                            upRecoilFrames = 2;
                            downRecoilFrames = 2;
                            exitVelocity = 1700;
                            magSize = 100 + Random.Range(0, 30);
                            totReloadFrames = 300;
                            shellAmount = 2;
                             shotCount = 1;
                            cycleType = cycleType.auto;
                            shotSpreadAngle = 1;
                            damage = 1;
                            break;

                        case weaponType.rpg:
                            ejectFrames = 80;
                            cycleFrames = 1;
                            maxRecoil = 150;
                            upRecoilFrames = 1;
                            downRecoilFrames = 45;
                            exitVelocity = 1800;
                            magSize = 1;
                            totReloadFrames = 500;
                            shellAmount = 1;
                             shotCount = 1;
                            cycleType = cycleType.auto;
                            shotSpreadAngle = 1;
                            damage = 300;
                            break;

                        case weaponType.rifle:
                            ejectFrames = 2;
                            cycleFrames = 1;
                            maxRecoil = 105;
                            upRecoilFrames = 1;
                            downRecoilFrames = 27;
                            exitVelocity = 2000;
                            magSize = 1;
                            totReloadFrames = 12;
                            shellAmount = 1;
                             shotCount = 1;
                            cycleType = cycleType.revolver;
                            shotSpreadAngle = 1;
                            damage = 25;
                            break;

                    }
                    break;

            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
