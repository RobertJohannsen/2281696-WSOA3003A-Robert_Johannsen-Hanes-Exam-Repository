using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camControl : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(plyCont.plycont.ply.transform.position.x, plyCont.plycont.ply.transform.position.y, -20);
    }
}
