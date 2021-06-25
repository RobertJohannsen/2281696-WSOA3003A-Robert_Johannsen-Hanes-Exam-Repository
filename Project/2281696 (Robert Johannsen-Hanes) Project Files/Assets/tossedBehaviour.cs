using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tossedBehaviour : MonoBehaviour
{
    public Rigidbody rb;
    int Speed;
    // Start is called before the first frame update
    void Start()
    {
     
        this.transform.localScale = plyCont.plycont.weapSpr.transform.localScale;
        this.GetComponent<SpriteRenderer>().sprite = plyCont.plycont.previousSprite;

        rb = this.GetComponent<Rigidbody>();
        this.transform.localRotation = Quaternion.Euler(0, 0, -plyCont.plycont.bulletAngle);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
