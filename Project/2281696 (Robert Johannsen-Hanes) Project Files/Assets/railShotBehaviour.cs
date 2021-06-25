using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class railShotBehaviour : MonoBehaviour
{
    public int stasisTime, currentTime;
    public bool debounce;
    public bool shotActive, hasHit;
    public GameObject railHit;
    // Start is called before the first frame update
    void Start()
    {
        stasisTime = 60;
        debounce = true;
        this.transform.localRotation = Quaternion.Euler(0, 0, -plyCont.plycont.bulletAngle);

       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentTime++;

        if (currentTime > stasisTime)
        {
           
                Destroy(this.gameObject);
           


        }
    }

    void Update()
    {
        if (currentTime < 20)
        {
            shotActive = true;
        }
        else
        {
            shotActive = false;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(shotActive)
        {
            if (!hasHit)
            {
                if (col.gameObject.tag == "enemy")
                {
                    CameraShaker.Instance.ShakeOnce(15f, 10f, 0.5f, 5f);
                    hasHit = true;
                    Instantiate(railHit, col.ClosestPointOnBounds(this.transform.position), Quaternion.identity);
                }

               
            }
        }
      
      
    }
}
