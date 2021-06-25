using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoTrack : MonoBehaviour
{
    public LineRenderer lr;
    public GameObject bulletPrefab,chargeAnim,_charge;
    public float shootThreshold,shootCount,plyAngle,lifeTime , lifeThres;
    public bool inRange,shoot,chargeAnimDebounce;
    public Vector3 dir,orig;

    public int shootNowThreshold, shootNowCount;
    // Start is called before the first frame update
    void Start()
    {

        lifeThres = 15;
        shootNowThreshold = 2;
        lr = this.GetComponent<LineRenderer>();

        chargeAnimDebounce = true;
    }

    // Update is called once per frame
    void Update()
    {
        orig = new Vector3(this.transform.position.x, this.transform.position.y, -4);
        if(!shoot)
        {
            dir = plyCont.plycont.ply.transform.position - orig;
            plyAngle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

        }




        if ((plyCont.plycont.ply.transform.position - orig).magnitude < 18)
        {
            lr.SetPosition(0, this.transform.position);
            lr.SetPosition(1, plyCont.plycont.ply.transform.position);
            inRange = true;
        }
        else
        {
            inRange = false;
            lr.SetPosition(0, this.transform.position);
            lr.SetPosition(1, this.transform.position);
        }
     
    }

    void FixedUpdate()
    {

        lifeTime += Time.deltaTime;

        if(lifeTime > lifeThres)
        {
            Destroy(_charge);
            Destroy(this.gameObject);
        }

        if(inRange)
        {
            if(chargeAnimDebounce)
            {

                chargeAnimDebounce = false;
                _charge = Instantiate(chargeAnim, this.transform.position, Quaternion.identity);
            }
            shootCount += Time.deltaTime;
            if(shootCount > shootThreshold)
            {
                chargeAnimDebounce = true;
                Destroy(_charge);
                shootCount = 0;
                shoot = true;
            }
        }
        else
        {

            chargeAnimDebounce = true;
            Destroy(_charge);
            shootCount = 0;
        }
       
        if(shoot)
        {
            shootNowCount++;
            if(shootNowCount > shootNowThreshold)
            {
                RaycastHit hit;
                if(Physics.Raycast(orig,dir , out hit, Mathf.Infinity))
                {
                    Debug.DrawRay(transform.position, dir, Color.yellow);
                    Debug.Log(hit.collider.gameObject);
                    Instantiate(bulletPrefab, hit.transform.position, Quaternion.identity);
                    Destroy(_charge);
                    Destroy(this.gameObject);
                }
            }
        }
    }

}
