using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public AudioSource effect;

    //Variables para controlar el disparo del arma.
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 20f;
   
   
    public void Fire(float damage){       
        GameObject bullet=Instantiate(bulletPrefab,firePoint.position,firePoint.rotation);
        bullet.GetComponent<bullet>().SetDamage(damage);
        effect.Play();
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up *fireForce,ForceMode2D.Impulse);
    }
}
