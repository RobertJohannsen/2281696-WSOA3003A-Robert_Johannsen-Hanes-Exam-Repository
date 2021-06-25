using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBehaviour : MonoBehaviour
{
   void OnTriggerEnter(Collider col)
    {
        if(col.tag == "enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
