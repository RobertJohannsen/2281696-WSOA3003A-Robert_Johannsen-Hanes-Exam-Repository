using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour
{

   
    [Header("Projectile Settings")]
    public int numberofProjectiles;
    public float projectileSpeed;
    public GameObject ProjectilePrefab;
    public float radius;
    public Vector3 startPoint;
    public static bulletController bCont;

    void Awake()
    {
        bCont = this;
    }


    public void SpawnProjectiles(int _noOfBullets, float _bulletSpeed, float _bulletRadius, Vector3 _startPoint, float _startAngle, float _maxAngle, bool _radial, GameObject proj)
    {


        float angle;
        angle = 0;
        if (_radial)
        {

            if (_noOfBullets % 2 == 0)
            {
                angle = _startAngle - (_maxAngle / 2);
            }
            else
            {
                int x = _noOfBullets / 2;


                angle = _startAngle - ((_maxAngle / _noOfBullets) * x);
            }
        }
        else
        {

            angle = _startAngle;
        }
        float angleStep = _maxAngle / _noOfBullets;

        for (int i = 0; i < _noOfBullets; i++)
        {
            //Direction Calculation
            float bulletDirXPosition = _startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * _bulletRadius;
            float bulletDirYPosition = _startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * _bulletRadius;

            Vector3 bulletVector = new Vector3(bulletDirXPosition, bulletDirYPosition, 0.6f);
            Vector3 bulletMoveDirection = (bulletVector - _startPoint).normalized * _bulletSpeed;

            GameObject tmpObj = Instantiate(proj, bulletVector, Quaternion.identity);
            // WaveController.waveCont.bullets[WaveController.waveCont.refIndex] = tmpObj;
            tmpObj.GetComponent<Rigidbody>().velocity = new Vector3(bulletMoveDirection.x, bulletMoveDirection.y, 0);
            // WaveController.waveCont.refIndex++;

            angle += angleStep;
        }

    }

}