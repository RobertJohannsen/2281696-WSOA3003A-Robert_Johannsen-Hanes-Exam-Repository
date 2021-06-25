using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletcide : MonoBehaviour
{
    public Transform ply;
    void Start()
    {
        ply = plyCont.plycont.ply.transform;
    }
    void FixedUpdate()
    {

        if ((ply.position - this.transform.position).magnitude >= 60)
        {
            Die();
        }
    }

    public void Die()
    {
    
        Destroy(this.gameObject);
    }
}
