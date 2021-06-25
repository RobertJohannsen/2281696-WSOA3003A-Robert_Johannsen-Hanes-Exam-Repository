using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sweepTellBehaviour : MonoBehaviour
{
    public GameObject leftSweep, rightSweep;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SweepLeft()
    {
        Instantiate(leftSweep, new Vector3(100, 90, 0), Quaternion.identity); 
    }

    public void SweepRight()
    {
        Instantiate(rightSweep, new Vector3(-100, 90, 0), Quaternion.identity);
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
