using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class rocketBehaviour : MonoBehaviour
{
    public Rigidbody rb;
    int rocketSpeed,baseRocketSpeed , maxRocketRampUp ,rocketRampUp , timeToRampUp , timeElapsed , rocketRate;
    public GameObject explody;
    public bool ignore;
    // Start is called before the first frame update
    void Start()
    {
        baseRocketSpeed = 20;
        maxRocketRampUp = 30;
        timeToRampUp = 30;
        rocketRate = 2;
        rb = this.GetComponent<Rigidbody>();
        this.transform.localRotation = Quaternion.Euler(0, 0, -plyCont.plycont.bulletAngle);
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed++;
        if(timeElapsed > timeToRampUp)
        {
            if(rocketSpeed < maxRocketRampUp)
            {
                rocketRampUp += rocketRate;
            }
          
            rocketSpeed = baseRocketSpeed + rocketRampUp;
        }
        rb.velocity = this.transform.up * rocketSpeed;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "EnemyBullet")
        {
            ignore = false;
        }

        if(col.gameObject.tag == "hole")
        {
            ignore = false;
        }

        if(col.gameObject.tag == "enemy")
        {
            ignore = true;
        }


        if(ignore)
        {
            GameObject ekusu = Instantiate(explody, col.ClosestPointOnBounds(this.transform.position), Quaternion.identity);
            if (plyCont.plycont.gunDamage == 20)
            {
                ekusu.transform.localScale = new Vector3(1, 1, 1);
            }
            if (plyCont.plycont.gunDamage == 45)
            {
                ekusu.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            }

            CameraShaker.Instance.ShakeOnce(10f, 7f, 0.1f, 2);

            Destroy(this.gameObject);
        }
      
    }
}
