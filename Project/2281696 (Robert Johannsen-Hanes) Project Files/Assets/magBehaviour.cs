using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magBehaviour : MonoBehaviour
{
    public Rigidbody rb;
    public int stasisTime, currentTime;
    // Start is called before the first frame update
    void Start()
    {
        stasisTime = 20;
        rb = this.GetComponent<Rigidbody>();

        rb.angularVelocity = new Vector3(0, 0, Random.Range(-5, 5));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentTime < stasisTime)
        {
            currentTime++;
        }
        else
        {
           
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
