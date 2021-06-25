using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContCanvCont : MonoBehaviour
{
    public MainCanvCont maincanvcont;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ContBtn()
    {
        maincanvcont.showCont = !maincanvcont.showCont;
    }
}
