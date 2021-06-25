using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grouyndcrackbehaviour : MonoBehaviour
{
    public GameObject enemyBullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void sumoonUpbullet()
    {
        bulletController.bCont.SpawnProjectiles(1, 200, 0.1f, this.transform.position, 0, 10, true, enemyBullet);
    }
}
