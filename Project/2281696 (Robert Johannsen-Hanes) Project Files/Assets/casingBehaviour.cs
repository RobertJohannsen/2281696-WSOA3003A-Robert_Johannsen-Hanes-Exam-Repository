using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class casingBehaviour : MonoBehaviour
{
    public  Rigidbody rb;
    public int stasisTime, currentTime;
    public bool debounce;
    // Start is called before the first frame update
    void Start()
    {
        stasisTime = 60;
        rb = this.GetComponent<Rigidbody>();

        rb.angularVelocity = new Vector3(0, 0, Random.Range(50, 90) * -1);
        debounce = true;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentTime++;

            if(currentTime > stasisTime)
        {
            if(debounce)
            {
                int randAngle = Random.Range(75, 90);
                rb.constraints = RigidbodyConstraints.FreezeAll;
                this.transform.localRotation = Quaternion.Euler(0, 0,randAngle );
                debounce = false;
            }
          
            
        }
    }
}
