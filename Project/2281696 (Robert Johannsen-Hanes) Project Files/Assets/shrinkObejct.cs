using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shrinkObejct : MonoBehaviour
{
    public bool shrinkObj;
    public Vector3 storedScale;
    float shrinkcount, literallyjustcringe;
    

    // Start is called before the first frame update
    void Start()
    {
        storedScale = this.transform.localScale;
        literallyjustcringe = 60;
        shrinkcount = literallyjustcringe;
    }

    // Update is called once per frame
    void Update()
    {
        if(shrinkObj)
        {
            if (shrinkcount > 0)
            {
                shrinkcount--;
                this.transform.localScale = storedScale * (shrinkcount / literallyjustcringe);

            }

            if (shrinkcount == 0)
            {
                Destroy(this.gameObject);
                
            }
        }
    }
}
