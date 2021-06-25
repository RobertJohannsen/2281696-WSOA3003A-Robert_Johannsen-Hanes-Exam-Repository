using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holeBehaviour : MonoBehaviour
{
    public GameObject storedObject;
    public bool startShinking = false;
    public float shrinkcount, literallyjustcringe;
    public Vector3 storedScale;
    public bool debounceStored = false;

    // Start is called before the first frame update
    void Start()
    {
        literallyjustcringe = 120;
        shrinkcount = literallyjustcringe;
    }

    // Update is called once per frame
    void Update()
    {
        if(startShinking)
        {
            if (shrinkcount > 0)
            {
                shrinkcount--;
                plyCont.plycont.ply.transform.localScale = storedScale * (shrinkcount / literallyjustcringe);

            }

            if(shrinkcount == 0)
            {
             
                resetHole();
                plyCont.plycont.playerDied("hole");
            }
           
        }
    }

    void resetHole()
    {
        shrinkcount = literallyjustcringe;
        startShinking = false;
    }

    void OnTriggerEnter(Collider col)
    {
        switch (col.gameObject.tag)
        {
            case "Player":
                plyCont.plycont.stuck = true;
                storedObject = col.gameObject;
                startShinking = true;
                col.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                if(!debounceStored)
                {
                    storedScale = col.gameObject.transform.localScale;
                }

                debounceStored = true;
                break;
            case "casing":
                col.gameObject.GetComponent<shrinkObejct>().shrinkObj = true;
                break;
        }

    }
}
