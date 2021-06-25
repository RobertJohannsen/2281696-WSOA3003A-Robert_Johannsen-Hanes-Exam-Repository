using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossMitosis : MonoBehaviour
{
    public enum enemymoveState { movetoTarget, sit, jiggle, chase };
    public enemymoveState movestate;
    public Vector3 targetpos;
    public float moveSpeed,plyAngle;
    public bool debounceSpread,debounceShoot;
    public GameObject bulletPrefab;
    // Start is called before the first frame update

    void Start()
    {
        debounceSpread = true;
        moveSpeed = 1.2f;
        movestate = enemymoveState.movetoTarget;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = plyCont.plycont.ply.transform.position - this.transform.position;
        plyAngle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

        switch (movestate)
        {
            case enemymoveState.movetoTarget:
                this.transform.position = Vector3.MoveTowards(this.transform.position, targetpos, moveSpeed);
                break;
            case enemymoveState.sit:
                break;
         
        }

        functionality();
    }

    void functionality()
    {

        if(!EnemyAi.enAI.mitosisSpreadOut)
        {
            debounceSpread = true;
        }
        if(EnemyAi.enAI.mitosisSpreadOut)
        {
            if(debounceSpread)
            {
                debounceSpread = false;
                targetpos = new Vector3(EnemyAi.enAI.gameObject.transform.position.x+(Random.Range(-10f,10f)), EnemyAi.enAI.gameObject.transform.position.y + (Random.Range(-10f, 10f)),this.transform.position.z);

            }
        }

        if (!EnemyAi.enAI.mitosisShoot)
        {
            debounceShoot = true;
        }
        if (EnemyAi.enAI.mitosisShoot)
        {
           
            if (debounceShoot)
            {
                debounceShoot = false;
                bulletController.bCont.SpawnProjectiles(1, 200, 0.1f, this.transform.position, plyAngle, 20, true, bulletPrefab);

            }
        }

     
        if (EnemyAi.enAI.mitosisCombine)
        {
                targetpos = new Vector3(EnemyAi.enAI.gameObject.transform.position.x + (Random.Range(-5f, 5f)), EnemyAi.enAI.gameObject.transform.position.y + (Random.Range(-3f, 3f)), this.transform.position.z);
        }


        if(EnemyAi.enAI.mitosisDie)
        {
            Destroy(this.gameObject);
        }
    }

   

}
