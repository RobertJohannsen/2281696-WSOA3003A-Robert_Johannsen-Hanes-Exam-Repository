using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p2BossAI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (!EnemyAi.enAI.Invulnerable)
        {

            if (col.gameObject.tag == "bullet")
            {

                EnemyAi.enAI.damageCount += plyCont.plycont.gunDamage;
                EnemyAi.enAI.currentHP -= plyCont.plycont.gunDamage;

            }

            if (col.gameObject.tag == "rail")
            {

                if (col.gameObject.GetComponent<railShotBehaviour>().shotActive)
                {

                    EnemyAi.enAI.damageCount += plyCont.plycont.gunDamage;
                    EnemyAi.enAI.currentHP -= plyCont.plycont.gunDamage;

                }

            }


        }
    }
}
