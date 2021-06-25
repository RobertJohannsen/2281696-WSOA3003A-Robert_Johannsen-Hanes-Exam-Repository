using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class OrbitalLaserBehaviour : MonoBehaviour
{
    public int chaseLag, chaselagCount;
    public float moveSpeed, lifeTime;
    public CameraShakeInstance shook;
    public GameObject boss;
    public int LifeThreshold;

    // Start is called before the first frame update
    void Start()
    {
        CameraShaker.Instance.ShakeOnce(20f, 20f, .1f, 12f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!plyCont.plycont.plyDied)
        {

            chaselagCount++;
            if (chaselagCount >= chaseLag)
            {
                if ((plyCont.plycont.ply.transform.position - this.transform.position).magnitude < 15)
                {
                    CameraShaker.Instance.ShakeOnce(5f, 4f, .1f, 1f);
                }


                chaselagCount = 0;
                this.transform.position = Vector3.MoveTowards(this.transform.position, plyCont.plycont.ply.transform.position, moveSpeed);
            }
        }
    }



    void FixedUpdate()
    {
        if(!plyCont.plycont.plyDied)
        {
            if (lifeTime < LifeThreshold)
            {
                lifeTime += Time.deltaTime;
            }
            else
            {
                Destroy(this.gameObject);
            }

        }

    }
}



