using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponPickupBehaviour : MonoBehaviour
{
    public int weptype, weparchetype;
    public Rigidbody rb;
    public int stasisTime, currentTime;
    public bool debounce , active;
    // Start is called before the first frame update
    void Start()
    {
        stasisTime = 65;
        rb = this.GetComponent<Rigidbody>();
        weptype = Random.Range(1, 6);
        weparchetype = Random.Range(1, 4);
        debounce = true;
        active = false;

        rb.velocity = new Vector3(Random.Range(-10f, 10f), Random.Range(10f, 10f), 0);

    }

    // Update is called once per frame
    void Update()
    {
        if(EnemyAi.enAI.finalHP == 0)
        {
            Destroy(this.gameObject);
        }   
    }

    void FixedUpdate()
    {
        currentTime++;

        if (currentTime > stasisTime)
        {
            if (debounce)
            {
               
                rb.constraints = RigidbodyConstraints.FreezeAll;
                debounce = false;
                active = true;
            }


        }
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
