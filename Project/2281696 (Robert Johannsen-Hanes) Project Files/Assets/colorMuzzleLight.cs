using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorMuzzleLight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Light>().color = this.transform.GetComponentInParent<SpriteRenderer>().color;
    }
}
